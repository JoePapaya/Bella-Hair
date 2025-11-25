using System;
using System.Threading.Tasks;
using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BellaHair.Test;

internal class TestDbContextFactory : IDbContextFactory<BellaHairDbContext2>
{
    private readonly DbContextOptions<BellaHairDbContext2> _options;

    public TestDbContextFactory(DbContextOptions<BellaHairDbContext2> options)
    {
        _options = options;
    }

    public BellaHairDbContext2 CreateDbContext()
        => new BellaHairDbContext2(_options);
}

[NUnit.Framework.TestFixture]   // <-- fully qualified
public class CustomerApplicationServiceTest
{
    private IDataService _dataService = null!;
    private DbContextOptions<BellaHairDbContext2> _options = null!;
    private string _dbName = null!;

    [NUnit.Framework.SetUp]
    public void Setup()
    {
        _dbName = Guid.NewGuid().ToString();

        _options = new DbContextOptionsBuilder<BellaHairDbContext2>()
            .UseInMemoryDatabase(_dbName)
            .Options;

        var factory = new TestDbContextFactory(_options);
        _dataService = new EfDataService(factory);
    }

    [NUnit.Framework.Test]
    public async Task AddKundeAsync_ShouldPersistCustomer()
    {
        var kunde = new Kunde
        {
            Navn = "Anna",
            Telefon = "12345678",
            Email = "anna@test.com",
            Fødselsdag = new DateOnly(1995, 5, 10),
            LoyaltyTier = "Bronze"
        };

        await _dataService.AddKundeAsync(kunde);

        NUnit.Framework.Assert.That(_dataService.Kunder.Count, NUnit.Framework.Is.EqualTo(1));
        NUnit.Framework.Assert.That(_dataService.Kunder[0].Navn, NUnit.Framework.Is.EqualTo("Anna"));
    }
}
