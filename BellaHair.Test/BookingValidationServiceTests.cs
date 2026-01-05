using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BookingValidationServiceTests
    {
        private BookingValidationService _service = null!;
        private Mock<IDataService> _mockDataService = null!;

        [SetUp]
        public void Setup()
        {
            _mockDataService = new Mock<IDataService>();
            _service = new BookingValidationService(_mockDataService.Object);
        }

        [Test]
        public async Task ValidateAsync_ShouldThrow_WhenEmployeeHasOverlappingBooking()
        {
            var now = DateTime.Now;

            // Arrange
            var existingBooking = new Booking
            {
                BookingId = 1,
                MedarbejderId = 1,
                Tidspunkt = now.AddHours(1),
                Varighed = 60,
                Status = BookingStatus.Kommende
            };

            var newBooking = new Booking
            {
                BookingId = 2,
                MedarbejderId = 1,
                Tidspunkt = now.AddHours(1).AddMinutes(30),
                Varighed = 60,
                Status = BookingStatus.Kommende
            };

            // Mock data
            _mockDataService.Setup(d => d.Bookinger).Returns(new List<Booking> { existingBooking });

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.ValidateAsync(newBooking));

            Assert.That(ex.Message, Is.EqualTo("Der findes allerede en booking i dette tidsinterval."));
        }

        [Test]
        public async Task ValidateAsync_ShouldNotThrow_WhenNoOverlap()
        {
            var now = DateTime.Now;

            var existingBooking = new Booking
            {
                BookingId = 1,
                MedarbejderId = 1,
                Tidspunkt = now.AddHours(1),
                Varighed = 60,
                Status = BookingStatus.Kommende
            };

            var newBooking = new Booking
            {
                BookingId = 2,
                MedarbejderId = 1,
                Tidspunkt = now.AddHours(2),
                Varighed = 60,
                Status = BookingStatus.Kommende
            };

            _mockDataService.Setup(d => d.Bookinger).Returns(new List<Booking> { existingBooking });

            Assert.DoesNotThrowAsync(async () => await _service.ValidateAsync(newBooking));
        }
    }
}
