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
                Description = "5% rabat stamkunder med 5 til 9 besøg",
                Percentage = 0.05m,
                Aktiv = true,
                IsKampagne = false,
                RequiredLoyaltyTier = LoyaltyTier.Bronze

            },
            new Rabat
            {
                RabatId = 1002,
                Navn = "Stamkunde Sølv",
                Description = "10% rabat til stamkunder med 10 til 19 besøg",
                Percentage = 0.10m,
                Aktiv = true,
                IsKampagne = false,
                RequiredLoyaltyTier = LoyaltyTier.Silver

            },
          new Rabat
          {
              RabatId = 1003,
              Navn = "Stamkunde Guld",
              Description = "15% rabat til stamkunder med 20 eller flere besøg",
              Percentage = 0.15m,
              Aktiv = true,
              IsKampagne = false,
              RequiredLoyaltyTier = LoyaltyTier.Gold
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
                Percentage = null,
                MinimumBeløb = null,
                RequiredLoyaltyTier = null

            },

            new Rabat
            {
                RabatId = 2002,
                Navn = "Nytårsrabat",
                Description = "5% rabat i januar",
                Aktiv = true,
                IsKampagne = true,
                StartDato = new DateTime(2026, 1, 1),
                SlutDato = new DateTime(2026, 1, 31),
                Percentage = 0.05m,
                MinimumBeløb = null,
                RequiredLoyaltyTier = null
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

            // 🔹 Snapshot felter
            entity.Property(f => f.KundeNavn)
                  .HasMaxLength(200);

            entity.Property(f => f.KundeEmail)
                  .HasMaxLength(200);

            entity.Property(f => f.KundeTelefon)
                  .HasMaxLength(50);

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
                Navn = "Kendrick",
                Email = "kendrick@example.com",
                Telefon = "12345678",
                Adresse = "Comptonvej 1",
                Postnr = "2100",
                By = "København Ø",
                Fødselsdag = new DateOnly(1987, 6, 17)
            },
            new Kunde
            {
                KundeId = 2,
                Navn = "J. Cole",
                Email = "jcole@example.com",
                Telefon = "22334455",
                Adresse = "Dreamvillegade 2",
                Postnr = "8000",
                By = "Aarhus C",
                Fødselsdag = new DateOnly(1985, 1, 28)
            },
            new Kunde
            {
                KundeId = 3,
                Navn = "Drake",
                Email = "drake@example.com",
                Telefon = "99887766",
                Adresse = "OVO Allé 3",
                Postnr = "5000",
                By = "Odense C",
                Fødselsdag = new DateOnly(1986, 10, 24)
            }
        );

        // ---------- SEED: MEDARBEJDERE ----------
        modelBuilder.Entity<Medarbejder>().HasData(
            new Medarbejder
            {
                MedarbejderId = 1,
                Navn = "George Pikins"
            },
            new Medarbejder
            {
                MedarbejderId = 2,
                Navn = "Dak Prescot"
            },
            new Medarbejder
            {
                MedarbejderId = 3,
                Navn = "CeeDee Lamb"
            }
        );

        // ---------- SEED: BEHANDLINGER ----------
        modelBuilder.Entity<Behandling>().HasData(
            new Behandling
            {
                BehandlingId = 1,
                Navn = "Standard klip",
                Pris = 100m,
                Type = "Klip",
                VarighedMinutter = 30
            },
            new Behandling
            {
                BehandlingId = 2,
                Navn = "Herreklip",
                Pris = 80m,
                Type = "Klip",
                VarighedMinutter = 25
            },
            new Behandling
            {
                BehandlingId = 3,
                Navn = "Farve",
                Pris = 150m,
                Type = "Farve",
                VarighedMinutter = 60
            },
            new Behandling
            {
                BehandlingId = 4,
                Navn = "Balayage",
                Pris = 250m,
                Type = "Farve",
                VarighedMinutter = 120
            },
            new Behandling
            {
                BehandlingId = 5,
                Navn = "Kurbehandling",
                Pris = 60m,
                Type = "Kur",
                VarighedMinutter = 30
            }
        );

    }

}