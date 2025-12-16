using System;
using System.Threading.Tasks;
using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BellaHair.Test;

internal class TestDbContextFactory : IDbContextFactory<BellaHairDbContext>
{
    private readonly DbContextOptions<BellaHairDbContext> _options;

    public TestDbContextFactory(DbContextOptions<BellaHairDbContext> options)
    {
        _options = options;
    }

    public BellaHairDbContext CreateDbContext() => new BellaHairDbContext(_options);
}

[TestFixture]
public class CreateEntitiesTests
{
    private IDataService _dataService = null!;
    private DbContextOptions<BellaHairDbContext> _options = null!;
    private string _dbName = null!;

    [SetUp]
    public void Setup()
    {
        _dbName = Guid.NewGuid().ToString();
        _options = new DbContextOptionsBuilder<BellaHairDbContext>()
            .UseInMemoryDatabase(_dbName)
            .Options;

        var factory = new TestDbContextFactory(_options);
        _dataService = new EfDataService(factory);
    }

    [Test]
    public async Task AddKunde_ShouldPersistCustomer()
    {
        var kunde = new Kunde
        {
            Navn = "Anna",
            Telefon = "12345678",
            Email = "anna@test.com"
        };

        await _dataService.AddKundeAsync(kunde);

        Assert.That(_dataService.Kunder.Count, Is.EqualTo(1));
        Assert.That(_dataService.Kunder[0].Navn, Is.EqualTo("Anna"));
    }

    [Test]
    public async Task AddMedarbejder_ShouldPersistEmployee()
    {
        var medarbejder = new Medarbejder
        {
            Navn = "Peter",
            ErFreelancer = true
        };

        await _dataService.AddMedarbejderAsync(medarbejder);

        Assert.That(_dataService.Medarbejdere.Count, Is.EqualTo(1));
        Assert.That(_dataService.Medarbejdere[0].Navn, Is.EqualTo("Peter"));
    }

    [Test]
    public async Task AddBooking_ShouldPersistBooking_UsingDbContextDirectly()
    {

        await using var db = new BellaHairDbContext(_options);

        var kunde = new Kunde { Navn = "Anna" };
        var medarbejder = new Medarbejder { Navn = "Peter" };

        db.Kunder.Add(kunde);
        db.Medarbejdere.Add(medarbejder);
        await db.SaveChangesAsync();

        var booking = new Booking
        {
            KundeId = kunde.KundeId,
            MedarbejderId = medarbejder.MedarbejderId,
            Tidspunkt = new DateTime(2025, 1, 10, 10, 0, 0),
            Varighed = 60
        };


        db.Bookinger.Add(booking);
        await db.SaveChangesAsync();


        var savedBooking = await db.Bookinger.FirstOrDefaultAsync();
        Assert.IsNotNull(savedBooking);
        Assert.That(savedBooking.KundeId, Is.EqualTo(kunde.KundeId));
        Assert.That(savedBooking.MedarbejderId, Is.EqualTo(medarbejder.MedarbejderId));
    }






}
