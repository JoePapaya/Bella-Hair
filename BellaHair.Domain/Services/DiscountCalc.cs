using BellaHair.Domain.Entities;

namespace BellaHair.Domain.Services;

public static class DiscountCalc
{
    public static DiscountResult CalculateBestDiscount(
        decimal originalPrice,
        Kunde? kunde,
        string? valgtRabatCode,
        IEnumerable<Rabat> alleRabatter)
    {
        // Vi forventer, at alleRabatter allerede er filtreret
        // på Aktiv, dato, loyalitetskrav osv. af KALDEREN.
        var kandidater = alleRabatter?.ToList() ?? new List<Rabat>();

        // 1) Hvis ingen kandidater → ingen rabat
        if (!kandidater.Any())
        {
            return new DiscountResult
            {
                OriginalPrice = originalPrice,
                FinalPrice = originalPrice,
                AppliedDiscount = null
            };
        }

     

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
            if (rabat.MinimumBeløb.HasValue && originalPrice < rabat.MinimumBeløb.Value)
                continue;

            var final = rabat.Apply(originalPrice);

            if (final < bestFinal)
            {
                bestFinal = final;
                bestRabat = rabat;
            }
        }

        // 4) Hvis ingen rabat gav reel besparelse
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
