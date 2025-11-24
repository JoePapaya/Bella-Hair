using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
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
            return db.Bookinger.AsNoTracking().ToList();
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
    public async Task DeleteBookingAsync(int bookingId)
    {
        await using var db = CreateContext();
        var booking = await db.Bookinger.FindAsync(bookingId);
        if (booking is null) return;

        db.Bookinger.Remove(booking);
        await db.SaveChangesAsync();
    }
    
    public async Task<Booking?> GetBookingAsync(int bookingId)
    {
        await using var db = CreateContext();
        return await db.Bookinger
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


}
