using BellaHair.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BellaHair.Infrastructure;

public class BellaHairDbContext2 : DbContext
{
    public BellaHairDbContext2(DbContextOptions<BellaHairDbContext2> options)
        : base(options)
    {
    }

    public DbSet<Kunde> Kunder => Set<Kunde>();
    public DbSet<Medarbejder> Medarbejdere => Set<Medarbejder>();
    public DbSet<Behandling> Behandlinger => Set<Behandling>();
    public DbSet<Booking> Bookinger => Set<Booking>();
    public DbSet<Rabat> Rabatter => Set<Rabat>();




    public DbSet<Faktura> Fakturaer { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------- Behandling ----------
        modelBuilder.Entity<Behandling>(entity =>
        {
            entity.Property(b => b.Pris)
                  .HasPrecision(18, 2);   // 18 total cifre, 2 decimaler (fx 1234567890123,45)
        });

        // ---------- Rabat ----------
        modelBuilder.Entity<Rabat>(entity =>
        {
            entity.Property(r => r.Percentage)
                  .HasPrecision(5, 2);    // fx 100,00 eller 12,50 %

            // hvis Rabat også har et beløbsfelt:
            // entity.Property(r => r.Beløb).HasPrecision(18, 2);
        });

        // ---------- Faktura ----------
        modelBuilder.Entity<Faktura>(entity =>
        {
            entity.Property(f => f.Beløb)
                  .HasPrecision(18, 2);

            entity.Property(f => f.RabatBeløb)
                  .HasPrecision(18, 2);

            entity.Property(f => f.TotalBeløb)
                  .HasPrecision(18, 2);

            entity.Property(f => f.RabatTekst)
                  .HasMaxLength(200);

            entity.HasOne(f => f.Kunde)
                  .WithMany()
                  .HasForeignKey(f => f.KundeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.Booking)
                  .WithMany()
                  .HasForeignKey(f => f.BookingId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

}