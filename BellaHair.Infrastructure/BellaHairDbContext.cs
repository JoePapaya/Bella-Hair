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
    public DbSet<Faktura> Fakturaer => Set<Faktura>();  

}

