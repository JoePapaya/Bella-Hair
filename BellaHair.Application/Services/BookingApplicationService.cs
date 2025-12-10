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
    private readonly IFakturaApplicationService _fakturaService;

    public BookingApplicationService(
        IDataService data,
        IBookingValidationService validator,
        ILoyaltyService loyaltyService,
        IFakturaApplicationService fakturaService)
    {
        _data = data;
        _validator = validator;
        _loyaltyService = loyaltyService;
        _fakturaService = fakturaService;
    }

    // ---------- Read ----------

    public Task<IList<Booking>> GetAllAsync()
        => Task.FromResult(_data.Bookinger);

    public Task<Booking?> GetByIdAsync(int id)
        => _data.GetBookingAsync(id);

    // ---------- Create ----------

    public async Task<Booking> CreateAsync(Booking booking)
    {
        // 1) Valider booking
        await _validator.ValidateAsync(booking);

        // 2) Gem booking
        var savedBooking = await _data.AddBookingAsync(booking);

        // 3) Hvis den oprettes direkte som Gennemført:
        if (savedBooking.Status == BookingStatus.Gennemført)
        {
            // → Loyalty
            await _loyaltyService.HandleBookingCompletedAsync(savedBooking.KundeId);

            // → Faktura
            await _fakturaService.EnsureForBookingAsync(savedBooking);
        }

        return savedBooking;
    }

    // ---------- Update ----------

    public async Task UpdateAsync(Booking booking)
    {
        // 0) Hent nuværende booking og faktura
        var eksisterende = await _data.GetBookingAsync(booking.BookingId);
        if (eksisterende is null)
            throw new InvalidOperationException($"Booking med id {booking.BookingId} blev ikke fundet.");

        var eksisterendeFaktura = await _fakturaService.GetForBookingAsync(booking.BookingId);

        var varAlleredeGennemført = eksisterende.Status == BookingStatus.Gennemført;
        var bliverNuGennemført = booking.Status == BookingStatus.Gennemført;

        // 🔒 Hvis der findes faktura og booking VAR gennemført, må status ikke ændres væk fra Gennemført
        if (eksisterendeFaktura != null &&
            varAlleredeGennemført &&
            booking.Status != BookingStatus.Gennemført)
        {
            throw new InvalidOperationException(
                "Kan ikke ændre status på en booking, der allerede har en faktura. " +
                "Hvis der er sket en fejl, skal det håndteres med kreditnota eller manuelt."
            );
        }

        // 1) Valider ny version
        await _validator.ValidateAsync(booking);

        // 2) Gem ændringer
        await _data.UpdateBookingAsync(booking);

        // 3) Hent opdateret booking
        var opdateret = await _data.GetBookingAsync(booking.BookingId);
        if (opdateret is null)
            throw new InvalidOperationException("Booking kunne ikke genindlæses efter opdatering.");

        // 4) Overgang: ikke-gennemført → gennemført
        if (!varAlleredeGennemført && bliverNuGennemført)
        {
            if (opdateret.Status != BookingStatus.Gennemført)
            {
                throw new InvalidOperationException(
                    "Internt problem: booking er ikke gemt som 'Gennemført', så der kan ikke oprettes faktura."
                );
            }

            // → Loyalty
            await _loyaltyService.HandleBookingCompletedAsync(opdateret.KundeId);

            // → Faktura
            await _fakturaService.EnsureForBookingAsync(opdateret);
        }
    }

    // ---------- Delete ----------

    public async Task DeleteAsync(int id)
    {
        try
        {
            var booking = await _data.GetBookingAsync(id);
            if (booking is null)
            {
                return;
            }

            // 🔒 Domæneregel: må ikke slette gennemførte bookinger
            if (booking.Status == BookingStatus.Gennemført)
            {
                throw new InvalidOperationException(
                    "Kan ikke slette en gennemført booking, fordi der er oprettet en faktura. " +
                    "Lav i stedet en kreditnota eller håndter det manuelt.");
            }

            var kundeId = booking.KundeId;

            // Rå sletning (ingen regler her)
            await _data.DeleteBookingRawAsync(id);

            // efter sletning: lad loyalty-service reagere
            await _loyaltyService.HandleBookingDeletedAsync(kundeId);
        }
        catch (InvalidOperationException)
        {
            // Allerede en pæn fejltekst → bare boble videre
            throw;
        }
        catch (DbUpdateException ex)
        {
            var inner = ex.InnerException?.Message ?? ex.Message;
            throw new InvalidOperationException(
                "Der opstod en fejl ved sletning af bookingen: " + inner, ex);
        }
    }


}
