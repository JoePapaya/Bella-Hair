using System;
using System.Collections.Generic;
using System.Linq;
using BellaHair.Application.Interfaces;
using BellaHair.Application.Services;
using BellaHair.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace BellaHair.Test
{
    [TestFixture]
    public class KampagneRabatTest
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
        public void BeregnBedsteRabat_ShouldApplyOnlyIfWithinCampaignPeriod()
        {

            var today = new DateTime(2025, 1, 15);

            var rabatIndenfor = new Rabat
            {
                RabatId = 1,
                Navn = "Januar Kampagne",
                Aktiv = true,
                StartDato = new DateTime(2025, 1, 10),
                SlutDato = new DateTime(2025, 1, 20),
                Percentage = 0.10m,
                IsKampagne = true
            };

            var rabatUdenfor = new Rabat
            {
                RabatId = 2,
                Navn = "December Kampagne",
                Aktiv = true,
                StartDato = new DateTime(2024, 12, 1),
                SlutDato = new DateTime(2024, 12, 31),
                Percentage = 0.50m,
                IsKampagne = true
            };

            var kunde = new Kunde { KundeId = 1 };

            _mockDataService.Setup(d => d.Rabatter).Returns(new List<Rabat> { rabatIndenfor, rabatUdenfor });


            var resultat = _rabatService.BeregnBedsteRabat(100m, kunde, null, today);

            Assert.AreEqual(90m, resultat.FinalPrice); // Kun rabat indenfor perioden
            
        }

        [Test]
        public void BeregnBedsteRabat_ShouldNotApplyIfOutsideCampaignPeriod()
        {
            var dato = new DateTime(2025, 2, 1); // udenfor alle kampagner

            var rabat = new Rabat
            {
                RabatId = 1,
                Navn = "Januar Kampagne",
                Aktiv = true,
                StartDato = new DateTime(2025, 1, 10),
                SlutDato = new DateTime(2025, 1, 20),
                Percentage = 0.10m,
                IsKampagne = true
            };

            var kunde = new Kunde { KundeId = 1 };

            _mockDataService.Setup(d => d.Rabatter).Returns(new List<Rabat> { rabat });

            var resultat = _rabatService.BeregnBedsteRabat(100m, kunde, null, dato);

            Assert.AreEqual(100m, resultat.FinalPrice);
            Assert.IsNull(resultat.AppliedDiscountName);
        }
    }
}
