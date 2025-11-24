using BellaHair.Application.Interfaces;
using BellaHair.Application.Services;
using BellaHair.Domain.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BellaHair.Test;

[TestFixture]
public class LoyalitetsServiceTest
{
    private class FakeDataService : IDataService
    {
        public IList<Booking> Bookinger { get; set; } = new List<Booking>();

        public IList<Kunde> Kunder => new List<Kunde>();
        public IList<Behandling> Behandlinger => new List<Behandling>();
        public IList<Medarbejder> Medarbejdere => new List<Medarbejder>();
        public IList<Rabat> Rabatter => new List<Rabat>();

        public Task DeleteBookingAsync(int bookingId) => Task.CompletedTask;
        public Task<Booking?> GetBookingAsync(int bookingId) => Task.FromResult<Booking?>(null);
        public Task<Booking> AddBookingAsync(Booking booking) => Task.FromResult(booking);
        public Task UpdateBookingAsync(Booking booking) => Task.CompletedTask;

        public Task AddMedarbejderAsync(Medarbejder medarbejder) => Task.CompletedTask;
        public Task UpdateMedarbejderAsync(Medarbejder medarbejder) => Task.CompletedTask;
        public Task DeleteMedarbejderAsync(int medarbejderId) => Task.CompletedTask;
        public Task<Medarbejder?> GetMedarbejderAsync(int medarbejderId) => Task.FromResult<Medarbejder?>(null);

        public Task AddKundeAsync(Kunde kunde) => Task.CompletedTask;
        public Task UpdateKundeAsync(Kunde kunde) => Task.CompletedTask;
        public Task DeleteKundeAsync(int kundeId) => Task.CompletedTask;
        public Task<Kunde?> GetKundeAsync(int kundeId) => Task.FromResult<Kunde?>(null);

        public Task AddBehandlingAsync(Behandling behandling) => Task.CompletedTask;
        public Task UpdateBehandlingAsync(Behandling behandling) => Task.CompletedTask;
        public Task DeleteBehandlingAsync(int behandlingId) => Task.CompletedTask;
        public Task<Behandling?> GetBehandlingAsync(int behandlingId) => Task.FromResult<Behandling?>(null);
        public Task<Behandling?> GetBehandlingAsync(string navn) => Task.FromResult<Behandling?>(null);

        public Task<Rabat> AddRabatAsync(Rabat rabat) => Task.FromResult(rabat);
        public Task<Rabat> UpdateRabatAsync(Rabat rabat) => Task.FromResult(rabat);
        public Task DeleteRabatAsync(int id) => Task.CompletedTask;
        public Task<Rabat?> GetRabatAsync(int id) => Task.FromResult<Rabat?>(null);
    }

    [Test]
    public void BeregnLoyalitet_GiverBronzeVed5Gennemførte()
    {
        var kunde = new Kunde { KundeId = 1 };

        var fakeDb = new FakeDataService
        {
            Bookinger = Enumerable.Range(1, 5)
                .Select(_ => new Booking
                {
                    KundeId = 1,
                    Status = BookingStatus.Gennemført
                })
                .ToList()
        };

        var service = new LoyaltyService(fakeDb);

        var tier = service.BeregnLoyaltyTierForKunde(kunde);

        Assert.That(tier, Is.EqualTo("Bronze"));
    }

    [Test]
    public async Task OpdaterLoyalitet_OpdatererTierVedÆndring()
    {
        var kunde = new Kunde { KundeId = 1, LoyaltyTier = "None" };

        var fakeDb = new FakeDataService
        {
            Bookinger = Enumerable.Range(1, 10)
                .Select(_ => new Booking
                {
                    KundeId = 1,
                    Status = BookingStatus.Gennemført
                })
                .ToList()
        };

        var service = new LoyaltyService(fakeDb);

        await service.OpdaterLoyaltyTierAsync(kunde);

        Assert.That(kunde.LoyaltyTier, Is.EqualTo("Sølv"));
    }
}
