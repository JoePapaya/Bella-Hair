using BellaHair.Domain.Entities;
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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Behandling.Pris
        modelBuilder.Entity<Behandling>()
            .Property(b => b.Pris)
            .HasColumnType("decimal(18,2)");

        // Rabat.Percentage
        modelBuilder.Entity<Rabat>()
            .Property(r => r.Percentage)
            .HasPrecision(5, 2); // 0–100.00

        // Rabat.FixedAmount
        modelBuilder.Entity<Rabat>()
            .Property(r => r.FixedAmount)
            .HasColumnType("decimal(18,2)");

        // Rabat.MinimumBeløb
        modelBuilder.Entity<Rabat>()
            .Property(r => r.MinimumBeløb)
            .HasColumnType("decimal(18,2)");
    }
}

