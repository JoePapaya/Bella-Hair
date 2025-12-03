using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BellaHair.Infrastructure;

public class BellaHairDbContext : DbContext
{
    public BellaHairDbContext(DbContextOptions<BellaHairDbContext> options)
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
                  .HasPrecision(18, 2);   // 18 total cifre, 2 decimaler
        });

        // ---------- Rabat ----------
        modelBuilder.Entity<Rabat>(entity =>
        {
            entity.Property(r => r.Percentage)
                  .HasPrecision(5, 2);    // fx 100,00 eller 12,50 %
        });

        modelBuilder.Entity<Rabat>().HasData(
            new Rabat
            {
                RabatId = 1001,
                Navn = "Stamkunde Bronze",
                Description = "5% rabat til Bronze-stamkunder",
                Percentage = 0.05m,
                RequiredLoyaltyTier = "Bronze",
                Aktiv = true,
                IsKampagne = false
            },
            new Rabat
            {
                RabatId = 1002,
                Navn = "Stamkunde Sølv",
                Description = "10% rabat til Sølv-stamkunder",
                Percentage = 0.10m,
                RequiredLoyaltyTier = "Sølv",
                Aktiv = true,
                IsKampagne = false
            },
            new Rabat
            {
                RabatId = 1003,
                Navn = "Stamkunde Guld",
                Description = "15% rabat til Guld-stamkunder",
                Percentage = 0.15m,
                RequiredLoyaltyTier = "Guld",
                Aktiv = true,
                IsKampagne = false
            },
            new Rabat
            {
                RabatId = 2001,
                Navn = "Julekampagne",
                Description = "Julekampagne: 50 kr rabat på alle behandlinger",
                FixedAmount = 50m,
                Aktiv = true,
                IsKampagne = true,
                StartDato = new DateTime(DateTime.Now.Year, 12, 1),
                SlutDato = new DateTime(DateTime.Now.Year, 12, 24),
                RequiredLoyaltyTier = null,
                Percentage = null,
                MinimumBeløb = null,
            }
        );

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

        // ---------- SEED: KUNDER ----------
        modelBuilder.Entity<Kunde>().HasData(
            new Kunde
            {
                KundeId = 1,
                Navn = "Kendrick"
            },
            new Kunde
            {
                KundeId = 2,
                Navn = "J. Cole"
            },
            new Kunde
            {
                KundeId = 3,
                Navn = "Drake"
            }
        );

        // ---------- SEED: MEDARBEJDERE ----------
        modelBuilder.Entity<Medarbejder>().HasData(
            new Medarbejder
            {
                MedarbejderId = 1,
                Navn = "Mia"
            },
            new Medarbejder
            {
                MedarbejderId = 2,
                Navn = "Sara"
            },
            new Medarbejder
            {
                MedarbejderId = 3,
                Navn = "Jonas"
            }
        );

        // ---------- SEED: BEHANDLINGER ----------
        modelBuilder.Entity<Behandling>().HasData(
            new Behandling
            {
                BehandlingId = 1,
                Navn = "Standard klip",
                Pris = 100m
            },
            new Behandling
            {
                BehandlingId = 2,
                Navn = "Herreklip",
                Pris = 80m
            },
            new Behandling
            {
                BehandlingId = 3,
                Navn = "Farve",
                Pris = 150m
            },
            new Behandling
            {
                BehandlingId = 4,
                Navn = "Balayage",
                Pris = 250m
            },
            new Behandling
            {
                BehandlingId = 5,
                Navn = "Kurbehandling",
                Pris = 60m
            }
        );

    }

}