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

        try
        {
            db.Medarbejdere.Remove(existing);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Smid en pæn fejltekst videre til UI
            throw new InvalidOperationException(
                "Medarbejderen kan ikke slettes, fordi den er brugt i én eller flere bookinger eller fakturaer.",
                ex);
        }
    }

    public async Task<Medarbejder?> GetMedarbejderAsync(int medarbejderId)
    {
        await using var db = CreateContext();
        return await db.Medarbejdere
            .FirstOrDefaultAsync(m => m.MedarbejderId == medarbejderId);
    }

    // ---------- Booking ----------

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
    public async Task DeleteBookingRawAsync(int bookingId)
    {
        await using var db = CreateContext();
        await using var t = await db.Database.BeginTransactionAsync();

        var booking = await db.Bookinger.FindAsync(bookingId);
        if (booking == null)
            return;

        // Slet tilhørende faktura(er), hvis de findes
        var fakturaer = db.Fakturaer.Where(f => f.BookingId == bookingId);
        db.Fakturaer.RemoveRange(fakturaer);

        // Slet selve bookingen
        db.Bookinger.Remove(booking);

        await db.SaveChangesAsync();
        await t.CommitAsync();
    }

    public async Task<int> GetCompletedBookingsCountForKundeAsync(int kundeId)
    {
        await using var db = CreateContext();
        return await db.Bookinger
            .CountAsync(b => b.KundeId == kundeId && b.Status == BookingStatus.Gennemført);
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

        try
        {
            db.Kunder.Remove(existing);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                "Kunden kan ikke slettes, fordi den bruges i en booking eller faktura.",
                ex);
        }
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

    public async Task DeleteBehandlingAsync(int id)
    {
        using var db = CreateContext();
        var entity = await db.Behandlinger.FirstOrDefaultAsync(b => b.BehandlingId == id);
        if (entity is null) return;

        try
        {
            db.Behandlinger.Remove(entity);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                "Behandlingen kan ikke slettes, fordi den er brugt i én eller flere bookinger.",
                ex);
        }
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

        try
        {
            db.Rabatter.Remove(entity);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                "Rabatten kan ikke slettes, fordi den er brugt i én eller flere bookinger eller fakturaer.",
                ex);
        }
    }
    public async Task<Rabat?> GetRabatAsync(int id)
    {
        using var db = CreateContext();
        return await db.Rabatter.AsNoTracking()
            .FirstOrDefaultAsync(r => r.RabatId == id);
    }


    // ---------- Faktura ----------

    public async Task AddFakturaAsync(Faktura faktura)
    {
        await using var db = CreateContext();
        db.Fakturaer.Add(faktura);
        await db.SaveChangesAsync();
    }

    public async Task<Faktura?> GetFakturaForBookingAsync(int bookingId)
    {
        await using var db = CreateContext();
        return await db.Fakturaer
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.BookingId == bookingId);
    }

}
