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
        var today = DateTime.Today;

        // 1) Kun aktive rabatter
        var kandidater = alleRabatter
            .Where(r => r.Aktiv)
            .ToList();

        // 2) Loyalitetsfilter (samme logik som i din RabatService)
        kandidater = kandidater
            .Where(r =>
                string.IsNullOrWhiteSpace(r.RequiredLoyaltyTier) ||
                string.Equals(
                    kunde?.LoyaltyTier,
                    r.RequiredLoyaltyTier,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();

        // 3) Kampagne-periode
        kandidater = kandidater
            .Where(r => r.IsWithinCampaignPeriod(today))
            .ToList();

        // 4) Hvis kunden har skrevet en specifik rabatkode
        if (!string.IsNullOrWhiteSpace(valgtRabatCode))
        {
            var medKode = kandidater
                .Where(r =>
                    (!string.IsNullOrWhiteSpace(r.Code) &&
                     string.Equals(r.Code, valgtRabatCode, StringComparison.OrdinalIgnoreCase))
                    || r.RabatId.ToString() == valgtRabatCode)
                .ToList();

            // Hvis vi fandt nogen der matcher (kode eller ID) → brug kun dem
            if (medKode.Any())
                kandidater = medKode;
        }


        // 5) Hvis ingen kandidater → ingen rabat
        if (!kandidater.Any())
        {
            return new DiscountResult
            {
                OriginalPrice = originalPrice,
                FinalPrice = originalPrice,
                AppliedDiscount = null
            };
        }

        // 6) Find bedste rabat (laveste slutpris)
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

        // Hvis ingen rabat gav reel besparelse
        if (bestRabat == null)
        {
            return new DiscountResult
            {
                OriginalPrice = originalPrice,
                FinalPrice = originalPrice,
                AppliedDiscount = null
            };
        }

        // 7) Returnér resultat
        return new DiscountResult
        {
            OriginalPrice = originalPrice,
            FinalPrice = bestFinal,
            AppliedDiscount = bestRabat
        };
    }
}
