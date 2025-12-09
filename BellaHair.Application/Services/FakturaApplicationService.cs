using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums; 


namespace BellaHair.Application.Services;

public class FakturaApplicationService : IFakturaApplicationService
{
    private readonly IDataService _data;
    private readonly IRabatService _rabatService;

    public FakturaApplicationService(IDataService data, IRabatService rabatService)
    {
        _data = data;
        _rabatService = rabatService;
    }

    public Task<Faktura?> GetForBookingAsync(int bookingId)
        => _data.GetFakturaForBookingAsync(bookingId);

    public async Task<Faktura> EnsureForBookingAsync(Booking booking)
    {
        // 1) Find eksisterende faktura
        var eksisterende = await _data.GetFakturaForBookingAsync(booking.BookingId);
        if (eksisterende is not null)
            return eksisterende;

        // 2) Sikre at booking er gennemført
        if (booking.Status != BookingStatus.Gennemført)
        {
            throw new InvalidOperationException(
                "Kan kun oprette faktura for bookinger med status 'Gennemført'.");
        }

        // 3) Hent kunde, behandling og medarbejder (til snapshots)
        var kunde = await _data.GetKundeAsync(booking.KundeId);
        if (kunde is null)
            throw new InvalidOperationException($"Kunde med id {booking.KundeId} blev ikke fundet.");

        var behandling = await _data.GetBehandlingAsync(booking.BehandlingId);
        var medarbejder = await _data.GetMedarbejderAsync(booking.MedarbejderId);

        var grundBeløb = behandling?.Pris ?? 0m;

        // 4) Beregn rabat via RabatService (så al rabatlogik er samlet ét sted)
        var discountResult = _rabatService.BeregnBedsteRabat(
            grundBeløb,
            kunde,
            null,                   // ingen manuelt valgt rabatkode endnu
            booking.Tidspunkt       // lås rabat på booking-dato
        );

        var rabatBeløb = discountResult.OriginalPrice - discountResult.FinalPrice;
        if (rabatBeløb < 0) rabatBeløb = 0;

        // 5) Lav tekst til rabatten (samme stil som før)
        string? rabatTekst = null;
        var applied = discountResult.AppliedDiscount;

        if (applied is not null && rabatBeløb > 0)
        {
            if (!string.IsNullOrWhiteSpace(applied.Navn))
                rabatTekst = applied.Navn;
            else if (applied.Percentage is > 0)
                rabatTekst = $"{applied.Percentage.Value * 100:0.#}% rabat";
            else if (applied.FixedAmount is > 0)
                rabatTekst = $"{applied.FixedAmount.Value:0.##} kr rabat";
            else
                rabatTekst = "Rabat";
        }

        // 6) Byg faktura-snapshot
        var faktura = new Faktura
        {
            KundeId = booking.KundeId,
            BookingId = booking.BookingId,

            // datoer
            FakturaDato = booking.Tidspunkt,
            BookingTidspunkt = booking.Tidspunkt,

            // kunde-snapshot
            KundeNavn = kunde.Navn,
            KundeEmail = kunde.Email,
            KundeTelefon = kunde.Telefon,

            // behandling og medarbejder snapshots
            BehandlingNavn = behandling?.Navn,
            MedarbejderNavn = medarbejder?.Navn,

            // beløb
            Beløb = discountResult.OriginalPrice,
            RabatBeløb = rabatBeløb,
            TotalBeløb = discountResult.FinalPrice,

            RabatTekst = rabatTekst,

            // firma-info
            ErFirmafaktura = kunde.KundeType == KundeType.Firma,
            Firmanavn = kunde.KundeType == KundeType.Firma ? kunde.Firmanavn : null,
            Cvr = kunde.KundeType == KundeType.Firma ? kunde.Cvr : null
        };

        // 7) Gem faktura gennem data-laget
        await _data.AddFakturaAsync(faktura);

        return faktura;
    }

}

