using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Application.Services;

public class BookingApplicationService : IBookingApplicationService
{
    private readonly IDataService _data;
    private readonly IBookingValidationService _validator;
    private readonly ILoyaltyService _loyaltyService;

    public BookingApplicationService(
        IDataService data,
        IBookingValidationService validator,
        ILoyaltyService loyaltyService)
    {
        _data = data;
        _validator = validator;
        _loyaltyService = loyaltyService;
    }

    // ---------- Read ----------

    public Task<IList<Booking>> GetAllAsync()
        => Task.FromResult(_data.Bookinger);

    public Task<Booking?> GetByIdAsync(int id)
        => _data.GetBookingAsync(id);

    // ---------- Create ----------

    public async Task<Booking> CreateAsync(Booking booking)
    {
        // 1) Valider booking (tjek overlap, tider osv.)
        await _validator.ValidateAsync(booking);

        // 2) Gem booking
        var savedBooking = await _data.AddBookingAsync(booking);

        // 3) Hvis den oprettes direkte som Gennemført:
        if (savedBooking.Status == BookingStatus.Gennemført)
        {
            // Opdater loyalty for kunden (Bronze/Sølv/Guld ud fra antal gennemførte)
            var kunde = await _data.GetKundeAsync(savedBooking.KundeId);
            if (kunde is not null)
            {
                await _loyaltyService.OpdaterLoyaltyTierAsync(kunde);
            }

            // Opret faktura hvis der ikke allerede findes en
            var eksisterendeFaktura = await _data.GetFakturaForBookingAsync(savedBooking.BookingId);
            if (eksisterendeFaktura == null)
            {
                await _data.CreateFakturaAsync(savedBooking);
            }
        }

        // Hvis status IKKE er Gennemført, laver vi hverken loyalty-opdatering eller faktura endnu
        return savedBooking;
    }

    // ---------- Update ----------

    public async Task UpdateAsync(Booking booking)
    {
        // 0) Hent den nuværende booking og evt. faktura fra databasen
        var eksisterende = await _data.GetBookingAsync(booking.BookingId);
        if (eksisterende is null)
            throw new InvalidOperationException($"Booking med id {booking.BookingId} blev ikke fundet.");

        var eksisterendeFaktura = await _data.GetFakturaForBookingAsync(booking.BookingId);

        var varAlleredeGennemført = eksisterende.Status == BookingStatus.Gennemført;
        var bliverNuGennemført = booking.Status == BookingStatus.Gennemført;

        // 🔒 Regel: hvis der findes faktura og booking VAR gennemført,
        // så må status ikke ændres til noget andet end Gennemført.
        if (eksisterendeFaktura != null &&
            varAlleredeGennemført &&
            booking.Status != BookingStatus.Gennemført)
        {
            throw new InvalidOperationException(
                "Kan ikke ændre status på en booking, der allerede har en faktura. " +
                "Hvis der er sket en fejl, skal det håndteres med kreditnota eller manuelt."
            );
        }

        // 1) Valider den nye version af booking (det er 'booking' fra UI)
        await _validator.ValidateAsync(booking);

        // 2) Gem ændringerne på booking
        await _data.UpdateBookingAsync(booking);

        // 🔄 HENT booking igen EFTER vi har gemt,
        // så vi er 100% sikre på at vi arbejder med den rigtige, opdaterede version
        var opdateret = await _data.GetBookingAsync(booking.BookingId);
        if (opdateret is null)
            throw new InvalidOperationException("Booking kunne ikke genindlæses efter opdatering.");

        // 3) Hvis booking går fra IKKE-gennemført → Gennemført:
        if (!varAlleredeGennemført && bliverNuGennemført)
        {
            // ekstra sikkerhed: tjek at den faktisk ER gennemført nu
            if (opdateret.Status != BookingStatus.Gennemført)
            {
                // Hvis du vil kan du ændre teksten, men det her er en mere ærlig fejl end den du får nu
                throw new InvalidOperationException(
                    "Internt problem: booking er ikke gemt som 'Gennemført', så der kan ikke oprettes faktura."
                );
            }

            // → Opdater loyalty tier for kunden
            var kunde = await _data.GetKundeAsync(opdateret.KundeId);
            if (kunde is not null)
            {
                await _loyaltyService.OpdaterLoyaltyTierAsync(kunde);
            }

            // → Sørg for at der findes en faktura (brug den OPDATERDE booking)
            var faktura = eksisterendeFaktura
                          ?? await _data.GetFakturaForBookingAsync(opdateret.BookingId);

            if (faktura == null)
            {
                await _data.CreateFakturaAsync(opdateret);
            }
        }

        // Hvis den fx bare får ny tid eller medarbejder, og status ikke ændrer sig
        // (eller forbliver Kommende), så rører vi ikke loyalty eller faktura.
    }


    // ---------- Delete ----------

    public async Task DeleteAsync(int id)
    {
        try
        {
            // 1) Find booking først, så vi ved hvilken kunde den tilhører
            var booking = await _data.GetBookingAsync(id);
            if (booking is null)
            {
                // Intet at slette
                return;
            }

            // Vi vil gerne kunne opdatere loyalty for kunden bagefter
            var kunde = await _data.GetKundeAsync(booking.KundeId);

            // 2) Slet booking via EfDataService (den nægter selv at slette Gennemført)
            await _data.DeleteBookingAsync(id);

            // 3) Efter sletning: opdater loyalty for kunden (ikke farligt,
            // da gennemførte bookinger i forvejen ikke må slettes)
            if (kunde is not null)
            {
                await _loyaltyService.OpdaterLoyaltyTierAsync(kunde);
            }
        }
        catch (DbUpdateException ex)
        {
            var inner = ex.InnerException?.Message ?? ex.Message;
            throw new Exception(inner, ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
        }
    }
}
