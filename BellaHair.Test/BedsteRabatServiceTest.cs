using System;
using System.Collections.Generic;
using System.Linq;
using BellaHair.Application.Interfaces;
using BellaHair.Application.Services;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums;
using NUnit.Framework;
using Moq;

namespace BellaHair.Test
{
    [TestFixture]
    public class BedsteRabatServiceTest
    {
        private Mock<IDataService> _mockDataService = null!;
        private RabatService _rabatService = null!;

        [SetUp]
        public void Setup()
        {
            _mockDataService = new Mock<IDataService>();
            _rabatService = new RabatService(_mockDataService.Object);
        }

        [Test]
        public void BeregnBedsteRabat_ShouldChooseHighestEligibleDiscount_BasedOnLoyalty()
        {
            // Arrange
            var kunde = new Kunde { KundeId = 1, LoyaltyTier = LoyaltyTier.Bronze };

            var rabatStamkundeBronze = new Rabat
            {
                RabatId = 1,
                Navn = "Bronze rabat",
                Percentage = 0.1m, // 10%
                RequiredLoyaltyTier = LoyaltyTier.Bronze
            };

            var rabatStamkundeSilver = new Rabat
            {
                RabatId = 2,
                Navn = "Silver rabat",
                Percentage = 0.2m, // 20%
                RequiredLoyaltyTier = LoyaltyTier.Silver
            };

            var rabatKampagne = new Rabat
            {
                RabatId = 3,
                Navn = "Kampagne rabat",
                Percentage = 0.15m, // 15%
                IsKampagne = true,
                StartDato = DateTime.Today.AddDays(-1),
                SlutDato = DateTime.Today.AddDays(1)
            };

            _mockDataService.Setup(d => d.Rabatter)
                .Returns(new List<Rabat> { rabatStamkundeBronze, rabatStamkundeSilver, rabatKampagne });

            // Act
            var resultat = _rabatService.BeregnBedsteRabat(100m, kunde, null, DateTime.Today);

            // Assert
            // Kunden er Bronze og kan få Bronze rabat og kampagnerabat
            Assert.AreEqual(85m, resultat.FinalPrice);
            Assert.AreEqual(rabatKampagne.RabatId, resultat.AppliedDiscount?.RabatId);
        }

        [Test]
        public void BeregnBedsteRabat_ShouldIgnoreIneligibleStamkundeDiscounts()
        {
            // Arrange
            var kunde = new Kunde { KundeId = 1, LoyaltyTier = LoyaltyTier.Bronze };

            var rabatSilver = new Rabat
            {
                RabatId = 1,
                Navn = "Silver rabat",
                Percentage = 0.2m,
                RequiredLoyaltyTier = LoyaltyTier.Silver
            };

            var rabatBronze = new Rabat
            {
                RabatId = 2,
                Navn = "Bronze rabat",
                Percentage = 0.1m,
                RequiredLoyaltyTier = LoyaltyTier.Bronze
            };

            _mockDataService.Setup(d => d.Rabatter)
                .Returns(new List<Rabat> { rabatSilver, rabatBronze });

            // Act
            var resultat = _rabatService.BeregnBedsteRabat(100m, kunde, null, DateTime.Today);

            // Assert
            // Kunden kan kun få Bronze rabat 
            Assert.AreEqual(90m, resultat.FinalPrice);
            Assert.AreEqual(rabatBronze.RabatId, resultat.AppliedDiscount?.RabatId);
        }

        [Test]
        public void BeregnBedsteRabat_ShouldChooseCampaignOverStamkundeIfHigher()
        {
            // Arrange
            var kunde = new Kunde { KundeId = 1, LoyaltyTier = LoyaltyTier.Bronze };

            var rabatBronze = new Rabat
            {
                RabatId = 1,
                Navn = "Bronze rabat",
                Percentage = 0.1m
            };

            var rabatKampagne = new Rabat
            {
                RabatId = 2,
                Navn = "Kampagne rabat",
                Percentage = 0.25m, // 25% større end stamkunde
                IsKampagne = true,
                StartDato = DateTime.Today.AddDays(-1),
                SlutDato = DateTime.Today.AddDays(1)
            };

            _mockDataService.Setup(d => d.Rabatter)
                .Returns(new List<Rabat> { rabatBronze, rabatKampagne });

            var resultat = _rabatService.BeregnBedsteRabat(200m, kunde, null, DateTime.Today);

      
            Assert.AreEqual(150m, resultat.FinalPrice);
            Assert.AreEqual(rabatKampagne.RabatId, resultat.AppliedDiscount?.RabatId);
        }
    }
}
