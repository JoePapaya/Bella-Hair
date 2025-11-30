using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums;
using BellaHair.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure;

public class EfDataService : IDataService
{
    private readonly IDbContextFactory<BellaHairDbContext> _factory;

    public EfDataService(IDbContextFactory<BellaHairDbContext> factory)
    {
        _factory = factory;
    }

    // Helper: always get a fresh context
    private BellaHairDbContext CreateContext() => _factory.CreateDbContext();

    // ---------- Lists ----------
    public IList<Booking> Bookinger
    {
        get
        {
            using var db = CreateContext();
            return db.Bookinger
                .Include(b => b.Kunde)
                .Include(b => b.Medarbejder)
                .Include(b => b.Behandling)
                .AsNoTracking()
                .ToList();
        }
    }

    public IList<Kunde> Kunder
    {
        get
        {
            using var db = CreateContext();
            return db.Kunder.AsNoTracking().ToList();
        }
    }

    public IList<Behandling> Behandlinger
    {
        get
        {
            using var db = CreateContext();
            return db.Behandlinger.AsNoTracking().ToList();
        }
    }

    public IList<Medarbejder> Medarbejdere
    {
        get
        {
            using var db = CreateContext();
            return db.Medarbejdere.AsNoTracking().ToList();
        }
    }

    public IList<Rabat> Rabatter
    {
        get
        {
            using var db = CreateContext();
            return db.Rabatter.AsNoTracking().ToList();
        }
    }

    public IList<Faktura> Fakturaer
    {
        get
        {
            using var db = CreateContext();
            return db.Fakturaer
                     .AsNoTracking()
                     .ToList();
        }
    }


    // ---------- Medarbejder ----------
    public async Task AddMedarbejderAsync(Medarbejder medarbejder)
    {
        await using var db = CreateContext();
        db.Medarbejdere.Add(medarbejder);
        await db.SaveChangesAsync();
    }

    public async Task UpdateMedarbejderAsync(Medarbejder medarbejder)
    {
        await using var db = CreateContext();
        db.Medarbejdere.Update(medarbejder);
        await db.SaveChangesAsync();
    }

    public async Task DeleteMedarbejderAsync(int medarbejderId)
    {
        await using var db = CreateContext();
        var existing = await db.Medarbejdere.FindAsync(medarbejderId);
        if (existing is null) return;

        db.Medarbejdere.Remove(existing);
        await db.SaveChangesAsync();
    }

    public async Task<Medarbejder?> GetMedarbejderAsync(int medarbejderId)
    {
        await using var db = CreateContext();
        return await db.Medarbejdere
            .FirstOrDefaultAsync(m => m.MedarbejderId == medarbejderId);
    }

    // ---------- Booking ----------
    public async Task DeleteBookingAsync(int bookkingid)
    {
        using var db = CreateContext();

        var booking = await db.Bookinger.FindAsync(bookkingid);
        if (booking == null)
            return;

        // 🔴 Må ikke slette gennemførte bookinger (de har betalt / faktura)
        if (booking.Status == BookingStatus.Gennemført)
        {
            throw new InvalidOperationException(
                "Kan ikke slette en gennemført booking, fordi der er oprettet en faktura. " +
                "Lav i stedet en kreditnota eller håndter det manuelt.");
        }

        // ✅ Booking er IKKE gennemført → her må vi godt rydde op

        // Slet tilhørende faktura(er), hvis de findes
        var fakturaer = db.Fakturaer.Where(f => f.BookingId == bookkingid);
        db.Fakturaer.RemoveRange(fakturaer);

        // Slet selve bookingen
        db.Bookinger.Remove(booking);

        await db.SaveChangesAsync();
    }

    public async Task<Booking?> GetBookingAsync(int bookingId)
    {
        await using var db = CreateContext();
        return await db.Bookinger
            .Include(b => b.Kunde)
            .Include(b => b.Medarbejder)
            .Include(b => b.Behandling)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public async Task<Booking> AddBookingAsync(Booking booking)
    {
        await using var db = CreateContext();
        db.Bookinger.Add(booking);
        await db.SaveChangesAsync();
        return booking;
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        await using var db = CreateContext();
        db.Bookinger.Update(booking);
        await db.SaveChangesAsync();
    }


    // ---------- Kunde ----------
    public async Task AddKundeAsync(Kunde kunde)
    {
        await using var db = CreateContext();
        db.Kunder.Add(kunde);
        await db.SaveChangesAsync();
    }

    public async Task UpdateKundeAsync(Kunde kunde)
    {
        await using var db = CreateContext();
        db.Kunder.Update(kunde);
        await db.SaveChangesAsync();
    }

    public async Task DeleteKundeAsync(int kundeId)
    {
        await using var db = CreateContext();
        var existing = await db.Kunder.FindAsync(kundeId);
        if (existing is null) return;

        db.Kunder.Remove(existing);
        await db.SaveChangesAsync();
    }

    public async Task<Kunde?> GetKundeAsync(int kundeId)
    {
        await using var db = CreateContext();
        return await db.Kunder
            .FirstOrDefaultAsync(k => k.KundeId == kundeId);
    }

    // ---------- Behandling ----------
    public async Task AddBehandlingAsync(Behandling behandling)
    {
        await using var db = CreateContext();
        db.Behandlinger.Add(behandling);
        await db.SaveChangesAsync();
    }

    public async Task UpdateBehandlingAsync(Behandling behandling)
    {
        await using var db = CreateContext();
        db.Behandlinger.Update(behandling);
        await db.SaveChangesAsync();
    }

    public async Task DeleteBehandlingAsync(int behandlingId)
    {
        await using var db = CreateContext();
        var existing = await db.Behandlinger.FindAsync(behandlingId);
        if (existing is null) return;
        db.Behandlinger.Remove(existing);
        await db.SaveChangesAsync();
    }

    public async Task<Behandling?> GetBehandlingAsync(int behandlingId)
    {
        await using var db = CreateContext();
        return await db.Behandlinger
            .FirstOrDefaultAsync(b => b.BehandlingId == behandlingId);
    }

    public async Task<Behandling?> GetBehandlingAsync(string navn)
    {
        await using var db = CreateContext();
        return await db.Behandlinger
            .FirstOrDefaultAsync(b => b.Navn == navn);
    }

    // ---------- Rabat ----------
    public async Task<Rabat> AddRabatAsync(Rabat rabat)
    {
        using var db = CreateContext();
        db.Rabatter.Add(rabat);
        await db.SaveChangesAsync();
        return rabat;
    }

    public async Task<Rabat> UpdateRabatAsync(Rabat rabat)
    {
        using var db = CreateContext();
        db.Rabatter.Update(rabat);
        await db.SaveChangesAsync();
        return rabat;
    }

    public async Task DeleteRabatAsync(int id)
    {
        using var db = CreateContext();
        var entity = await db.Rabatter.FirstOrDefaultAsync(r => r.RabatId == id);
        if (entity is null) return;

        db.Rabatter.Remove(entity);
        await db.SaveChangesAsync();
    }

    public async Task<Rabat?> GetRabatAsync(int id)
    {
        using var db = CreateContext();
        return await db.Rabatter.AsNoTracking()
            .FirstOrDefaultAsync(r => r.RabatId == id);
    }


    // ---------- Faktura ----------

    public async Task<Faktura> CreateFakturaAsync(Booking booking)
    {
        using var db = CreateContext();

        // Sørg for at vi har den nyeste booking fra databasen (med BookingId)
        var dbBooking = await db.Bookinger.FindAsync(booking.BookingId);
        if (dbBooking == null)
            throw new InvalidOperationException($"Booking med id {booking.BookingId} blev ikke fundet.");

        // 🔒 SIKKERHED: Faktura kun for gennemførte bookinger
        if (dbBooking.Status != BookingStatus.Gennemført)
        {
            throw new InvalidOperationException(
                "Kan kun oprette faktura for bookinger med status 'Gennemført'.");
        }

        // Tjek om der allerede findes en faktura til denne booking
        var eksisterende = await db.Fakturaer
            .FirstOrDefaultAsync(f => f.BookingId == dbBooking.BookingId);

        if (eksisterende is not null)
        {
            // Vi laver ikke en ny – vi genbruger den eksisterende
            return eksisterende;
        }

        // Find behandling for at få grundpris
        var behandling = await db.Behandlinger.FindAsync(dbBooking.BehandlingId);
        var grundBeløb = behandling?.Pris ?? 0m;

        // Find kunde
        var kunde = await db.Kunder.FindAsync(dbBooking.KundeId);

        // Brug bookingens dato som "historisk" dato til kampagner
        var dato = dbBooking.Tidspunkt.Date;

        // Filtrer rabatter historisk
        var alleRabatter = await db.Rabatter
            .Where(r => r.Aktiv)
            .ToListAsync();

        var kandidater = alleRabatter
            .Where(r => r.IsWithinCampaignPeriod(dato))
            .Where(r =>
                string.IsNullOrWhiteSpace(r.RequiredLoyaltyTier) ||
                string.Equals(
                    kunde?.LoyaltyTier,
                    r.RequiredLoyaltyTier,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();

        // Beregn bedste rabat
        var discountResult = DiscountCalc.CalculateBestDiscount(
            grundBeløb,
            kunde,
            dbBooking.ValgtRabat,
            kandidater);

        var rabatBeløb = discountResult.OriginalPrice - discountResult.FinalPrice;
        if (rabatBeløb < 0) rabatBeløb = 0; // safety

        string? rabatTekst = null;
        if (discountResult.AppliedDiscount is not null)
        {
            var d = discountResult.AppliedDiscount;
            rabatTekst = !string.IsNullOrWhiteSpace(d.Code)
                ? $"{d.Navn} ({d.Code})"
                : d.Navn;
        }

        var faktura = new Faktura
        {
            KundeId = dbBooking.KundeId,
            BookingId = dbBooking.BookingId,
            FakturaDato = dbBooking.Tidspunkt,

            Beløb = discountResult.OriginalPrice,
            RabatBeløb = rabatBeløb,
            TotalBeløb = discountResult.FinalPrice,

            RabatTekst = rabatTekst,
            ErFirmafaktura = false,
            Firmanavn = null,
            Cvr = null
        };

        db.Fakturaer.Add(faktura);
        await db.SaveChangesAsync();

        return faktura;
    }


    public async Task<Faktura?> GetFakturaForBookingAsync(int bookingId)
    {
        await using var db = CreateContext();
        return await db.Fakturaer
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.BookingId == bookingId);
    }
}
