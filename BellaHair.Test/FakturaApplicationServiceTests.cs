using System;
using System.Threading.Tasks;
using BellaHair.Application.Interfaces;
using BellaHair.Application.Services;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums;
using Moq;
using NUnit.Framework;

namespace BellaHair.Test
{
    [TestFixture]
    public class FakturaApplicationServiceTests
    {
        private Mock<IDataService> _mockDataService = null!;
        private Mock<IRabatService> _mockRabatService = null!;
        private FakturaApplicationService _fakturaService = null!;

        [SetUp]
        public void Setup()
        {
            _mockDataService = new Mock<IDataService>();
            _mockRabatService = new Mock<IRabatService>();
            _fakturaService = new FakturaApplicationService(_mockDataService.Object, _mockRabatService.Object);
        }

        [Test]
        public async Task EnsureForBookingAsync_ShouldCalculateTotalCorrectly()
        {
            // Arrange
            var booking = new Booking
            {
                BookingId = 1,
                KundeId = 1,
                BehandlingId = 1,
                MedarbejderId = 1,
                Tidspunkt = new DateTime(2025, 1, 10),
                Status = BookingStatus.Gennemført
            };

            var kunde = new Kunde
            {
                KundeId = 1,
                Navn = "Test Kunde",
                Email = "test@test.com",
                Telefon = "12345678",
                KundeType = KundeType.Privat,
                LoyaltyTier = LoyaltyTier.Bronze
            };

            var behandling = new Behandling
            {
                BehandlingId = 1,
                Navn = "Klippning",
                Pris = 500m
            };

            var medarbejder = new Medarbejder
            {
                MedarbejderId = 1,
                Navn = "Frisør"
            };

            var appliedRabat = new Rabat
            {
                Navn = "Bronze rabat",
                Percentage = 0.1m
            };

            var discountResult = new DiscountResult
            {
                OriginalPrice = behandling.Pris,
                FinalPrice = behandling.Pris * 0.9m, // 10% rabat
                AppliedDiscount = appliedRabat
            };

            _mockDataService.Setup(d => d.GetFakturaForBookingAsync(booking.BookingId)).ReturnsAsync((Faktura?)null);
            _mockDataService.Setup(d => d.GetKundeAsync(booking.KundeId)).ReturnsAsync(kunde);
            _mockDataService.Setup(d => d.GetBehandlingAsync(booking.BehandlingId)).ReturnsAsync(behandling);
            _mockDataService.Setup(d => d.GetMedarbejderAsync(booking.MedarbejderId)).ReturnsAsync(medarbejder);
            _mockDataService.Setup(d => d.AddFakturaAsync(It.IsAny<Faktura>())).Returns(Task.CompletedTask);

            _mockRabatService.Setup(r => r.BeregnBedsteRabat(
                behandling.Pris, kunde, null, booking.Tidspunkt))
                .Returns(discountResult);

            // Act
            var faktura = await _fakturaService.EnsureForBookingAsync(booking);

            // Assert
            Assert.AreEqual(500m, faktura.Beløb); // Original pris
            Assert.AreEqual(50m, faktura.RabatBeløb); // 10% rabat
            Assert.AreEqual(450m, faktura.TotalBeløb); // Total = Beløb - Rabat
            Assert.AreEqual("Bronze rabat", faktura.RabatTekst);
            Assert.AreEqual(kunde.Navn, faktura.KundeNavn);
            Assert.AreEqual(behandling.Navn, faktura.BehandlingNavn);
            Assert.AreEqual(medarbejder.Navn, faktura.MedarbejderNavn);
        }

        [Test]
        public void EnsureForBookingAsync_ShouldThrow_WhenBookingNotCompleted()
        {
            // Arrange
            var booking = new Booking
            {
                BookingId = 1,
                KundeId = 1,
                Status = BookingStatus.Kommende
            };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _fakturaService.EnsureForBookingAsync(booking));
        }
    }
}
