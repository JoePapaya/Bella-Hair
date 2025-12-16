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
    public class LoyaltyServiceTests
    {
        private Mock<IDataService> _mockDataService = null!;
        private LoyaltyService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockDataService = new Mock<IDataService>();
            _service = new LoyaltyService(_mockDataService.Object);
        }

        [TestCase(0, LoyaltyTier.None)]
        [TestCase(4, LoyaltyTier.None)]
        [TestCase(5, LoyaltyTier.Bronze)]
        [TestCase(9, LoyaltyTier.Bronze)]
        [TestCase(10, LoyaltyTier.Silver)]
        [TestCase(19, LoyaltyTier.Silver)]
        [TestCase(20, LoyaltyTier.Gold)]
        [TestCase(30, LoyaltyTier.Gold)]
        public async Task OpdaterLoyaltyTierAsync_ShouldSetCorrectTier(int completedBookings, LoyaltyTier expectedTier)
        {
            var kunde = new Kunde { KundeId = 1, LoyaltyTier = LoyaltyTier.None };

            _mockDataService.Setup(d => d.GetCompletedBookingsCountForKundeAsync(kunde.KundeId))
                .ReturnsAsync(completedBookings);

            _mockDataService.Setup(d => d.UpdateKundeAsync(It.IsAny<Kunde>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _service.OpdaterLoyaltyTierAsync(kunde);

            Assert.AreEqual(expectedTier, kunde.LoyaltyTier);

            if (kunde.LoyaltyTier != LoyaltyTier.None)
                _mockDataService.Verify(d => d.UpdateKundeAsync(It.IsAny<Kunde>()), Times.Once);
            else
                _mockDataService.Verify(d => d.UpdateKundeAsync(It.IsAny<Kunde>()), Times.Never);
        }

        [Test]
        public async Task HandleBookingCompletedAsync_ShouldUpdateTier_WhenKundeExists()
        {
            var kunde = new Kunde { KundeId = 1, LoyaltyTier = LoyaltyTier.None };
            _mockDataService.Setup(d => d.GetKundeAsync(kunde.KundeId)).ReturnsAsync(kunde);
            _mockDataService.Setup(d => d.GetCompletedBookingsCountForKundeAsync(kunde.KundeId)).ReturnsAsync(7);
            _mockDataService.Setup(d => d.UpdateKundeAsync(It.IsAny<Kunde>())).Returns(Task.CompletedTask);

            await _service.HandleBookingCompletedAsync(kunde.KundeId);

            Assert.AreEqual(LoyaltyTier.Bronze, kunde.LoyaltyTier);
            _mockDataService.Verify(d => d.UpdateKundeAsync(It.IsAny<Kunde>()), Times.Once);
        }

        [Test]
        public async Task HandleBookingCompletedAsync_ShouldDoNothing_WhenKundeIsNull()
        {
            _mockDataService.Setup(d => d.GetKundeAsync(It.IsAny<int>())).ReturnsAsync((Kunde?)null);

            Assert.DoesNotThrowAsync(async () => await _service.HandleBookingCompletedAsync(1));
        }
    }
}
