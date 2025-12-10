using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Infrastructure;

public class BellaHairDbContextFactory : IDesignTimeDbContextFactory<BellaHairDbContext>
{
    public BellaHairDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BellaHairDbContext>();

        // Use the SAME connection string as your Program.cs
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=BellaHairDb;Trusted_Connection=True;MultipleActiveResultSets=true");


        return new BellaHairDbContext(optionsBuilder.Options);
    }
}

//Command til at lave Migration
//Add-Migration InitialCreate
//Update-Database