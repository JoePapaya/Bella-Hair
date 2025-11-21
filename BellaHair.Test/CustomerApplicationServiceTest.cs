using System;
using System.Threading.Tasks;
using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


namespace BellaHair.Test
{
    // Simple factory for tests (because EfDataService wants IDbContextFactory)
    internal class TestDbContextFactory : IDbContextFactory<BellaHairDbContext>
    {
        private readonly DbContextOptions<BellaHairDbContext> _options;

        public TestDbContextFactory(DbContextOptions<BellaHairDbContext> options)
        {
            _options = options;
        }

        public BellaHairDbContext CreateDbContext()
            => new BellaHairDbContext(_options);
    }

    [TestFixture]
    public class CustomerApplicationServiceTest
    {
        private IDataService _dataService = null!;
        private DbContextOptions<BellaHairDbContext> _options = null!;
        private string _dbName = null!;

        [SetUp]
        public void Setup()
        {
            // Unique DB per test
            _dbName = Guid.NewGuid().ToString();

            _options = new DbContextOptionsBuilder<BellaHairDbContext>()
                .UseInMemoryDatabase(_dbName)
                .Options;

            var factory = new TestDbContextFactory(_options);
            _dataService = new EfDataService(factory);
        }

        [Test]
        public async Task AddKundeAsync_ShouldPersistCustomer()
        {
            // Arrange
            var kunde = new Kunde
            {
                Navn = "Anna",
                Telefon = "12345678",
                Email = "anna@test.com",
                Fødselsdag = new DateOnly(1995, 5, 10),
                LoyaltyTier = "Bronze"
            };

            // Act
            await _dataService.AddKundeAsync(kunde);

            // Assert (Kunder property should now contain it)
            Assert.That(_dataService.Kunder.Count, Is.EqualTo(1));
            Assert.That(_dataService.Kunder[0].Navn, Is.EqualTo("Anna"));
        }

        [Test]
        public async Task GetKundeAsync_ShouldReturnCustomer_WhenExists()
        {
            // Arrange
            var kunde = new Kunde { Navn = "Bella" };
            await _dataService.AddKundeAsync(kunde);

            var id = kunde.KundeId;

            // Act
            var fetched = await _dataService.GetKundeAsync(id);

            // Assert
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched!.Navn, Is.EqualTo("Bella"));
        }

        [Test]
        public async Task UpdateKundeAsync_ShouldChangeValues()
        {
            // Arrange
            var kunde = new Kunde { Navn = "Chris", Telefon = "11111111" };
            await _dataService.AddKundeAsync(kunde);

            kunde.Navn = "Chris Updated";
            kunde.Telefon = "22222222";

            // Act
            await _dataService.UpdateKundeAsync(kunde);
            var fetched = await _dataService.GetKundeAsync(kunde.KundeId);

            // Assert
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched!.Navn, Is.EqualTo("Chris Updated"));
            Assert.That(fetched.Telefon, Is.EqualTo("22222222"));
        }

        [Test]
        public async Task DeleteKundeAsync_ShouldRemoveCustomer()
        {
            // Arrange
            var kunde = new Kunde { Navn = "Delete Me" };
            await _dataService.AddKundeAsync(kunde);

            // Act
            await _dataService.DeleteKundeAsync(kunde.KundeId);

            // Assert
            Assert.That(_dataService.Kunder.Count, Is.EqualTo(0));
            var fetched = await _dataService.GetKundeAsync(kunde.KundeId);
            Assert.That(fetched, Is.Null);
        }
    }
}
