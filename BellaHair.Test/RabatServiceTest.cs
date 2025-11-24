using BellaHair.Domain.Entities;
using BellaHair.Domain.Services;
using NUnit.Framework;
using System.Collections.Generic;


namespace BellaHair.Test;

[TestFixture]
public class RabatServiceTest
{
    [Test]
    public void CalculateBestDiscount_ShouldApplyBestPercentage()
    {
        // Arrange
        decimal price = 100m;

        var kunde = new Kunde
        {
            KundeId = 1,
            LoyaltyTier = "Guld"
        };

        var rabatter = new List<Rabat>
        {
            new Rabat { Navn = "Bronze", Percentage = 0.05m, RequiredLoyaltyTier = "Bronze" },
            new Rabat { Navn = "Guld", Percentage = 0.15m, RequiredLoyaltyTier = "Guld" }
        };

        // Act
        var result = DiscountCalc.CalculateBestDiscount(price, kunde, null, rabatter);

        // Assert
        Assert.That(result.AppliedDiscount, Is.Not.Null);
        Assert.That(result.AppliedDiscount!.Navn, Is.EqualTo("Guld"));
        Assert.That(result.FinalPrice, Is.EqualTo(85m));
    }

    [Test]
    public void CalculateBestDiscount_ShouldNotApplyWrongLoyaltyDiscount()
    {
        // Arrange
        decimal price = 100m;

        var kunde = new Kunde
        {
            KundeId = 1,
            LoyaltyTier = "Bronze"
        };

        var rabatter = new List<Rabat>
        {
            new Rabat { Navn = "Guld", Percentage = 0.20m, RequiredLoyaltyTier = "Guld" }
        };

        // Act
        var result = DiscountCalc.CalculateBestDiscount(price, kunde, null, rabatter);

        // Assert
        Assert.That(result.AppliedDiscount, Is.Null);
        Assert.That(result.FinalPrice, Is.EqualTo(100m));
    }

    [Test]
    public void CalculateBestDiscount_ShouldApplySelectedCodeOnly()
    {
        // Arrange
        decimal price = 100m;

        var kunde = new Kunde
        {
            KundeId = 1,
            LoyaltyTier = "VIP"
        };

        var rabatter = new List<Rabat>
        {
            new Rabat { Navn = "VIP10", Percentage = 0.10m },
            new Rabat { Navn = "VIP30", Percentage = 0.30m }
        };

        // Act
        var result = DiscountCalc.CalculateBestDiscount(price, kunde, "VIP10", rabatter);

        // Assert
        Assert.That(result.AppliedDiscount, Is.Not.Null);
        Assert.That(result.AppliedDiscount!.Navn, Is.EqualTo("VIP10"));
        Assert.That(result.FinalPrice, Is.EqualTo(90m));
    }
}
