using BellaHair.Domain.Entities;

namespace BellaHair.Domain.Services;

public static class DiscountCalc
{
    // Fælles regel for om en rabat må bruges til en kunde
    public static bool IsRabatAllowedForKunde(Rabat rabat, Kunde? kunde)
    {
        // Stamkunde-rabatter: vi genkender dem på navnet
        // "Stamkunde Bronze", "Stamkunde Sølv", "Stamkunde Guld"
        bool isLoyalty =
            rabat.Navn.StartsWith("Stamkunde", StringComparison.OrdinalIgnoreCase);

        if (!isLoyalty)
        {
            // Kampagner og andre rabatter gælder for alle
            return true;
        }

        // Stamkunde-rabat → kræver at kunden har en tier
        if (kunde == null || string.IsNullOrWhiteSpace(kunde.LoyaltyTier))
            return false;

        // Kræv MATCH mellem rabat-navn og kundens tier
        // (Bronze → "Bronze", Sølv → "Sølv", Guld → "Guld")
        var tier = kunde.LoyaltyTier;

        if (rabat.Navn.Contains("Bronze", StringComparison.OrdinalIgnoreCase))
            return tier.Equals("Bronze", StringComparison.OrdinalIgnoreCase);

        if (rabat.Navn.Contains("Sølv", StringComparison.OrdinalIgnoreCase))
            return tier.Equals("Sølv", StringComparison.OrdinalIgnoreCase);

        if (rabat.Navn.Contains("Guld", StringComparison.OrdinalIgnoreCase))
            return tier.Equals("Guld", StringComparison.OrdinalIgnoreCase);

        // Hvis vi ikke kan tyde den → for en sikkerheds skyld ikke tilladt
        return false;
    }

    public static DiscountResult CalculateBestDiscount(
        decimal originalPrice,
        Kunde? kunde,
        string? valgtRabatCode,
        IEnumerable<Rabat> alleRabatter)
    {
        // alleRabatter er allerede filtreret på Aktiv/dato i RabatService/EfDataService
        var kandidater = alleRabatter?.ToList() ?? new List<Rabat>();

        // 1) Ingen rabatter → ingen rabat
        if (!kandidater.Any())
        {
            return new DiscountResult
            {
                OriginalPrice = originalPrice,
                FinalPrice = originalPrice,
                AppliedDiscount = null
            };
        }

        // 3) Find bedste rabat (laveste slutpris)
        Rabat? bestRabat = null;
        decimal bestFinal = originalPrice;

        foreach (var rabat in kandidater)
        {
            // Minimum-beløb
            if (rabat.MinimumBeløb.HasValue && originalPrice < rabat.MinimumBeløb.Value)
                continue;

            var final = rabat.Apply(originalPrice);

            if (final < bestFinal)
            {
                bestFinal = final;
                bestRabat = rabat;
            }
            else if (final == bestFinal && bestRabat is not null)
            {
                // Tie-breaker:
                // Hvis samme slutpris: stamkunde-rabat (IsKampagne == false) vinder over kampagne
                if (!rabat.IsKampagne && bestRabat.IsKampagne)
                {
                    bestRabat = rabat;
                }
            }
        }

        // 4) Hvis ingen reel besparelse → ingen rabat
        if (bestRabat == null || bestFinal >= originalPrice)
        {
            return new DiscountResult
            {
                OriginalPrice = originalPrice,
                FinalPrice = originalPrice,
                AppliedDiscount = null
            };
        }

        // 5) Returnér resultat
        return new DiscountResult
        {
            OriginalPrice = originalPrice,
            FinalPrice = bestFinal,
            AppliedDiscount = bestRabat
        };
    }
}
