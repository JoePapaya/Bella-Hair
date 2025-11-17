using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Entities;

namespace BellaHair.Domain.Services
{

    public static class DiscountCalc
    {
        public static DiscountResult CalculateBestDiscount(
            decimal originalPrice,
            Kunde? kunde,
            string? valgtRabatCode,
            IEnumerable<Rabat> alleRabatter)
        {
            var result = new DiscountResult
            {
                OriginalPrice = originalPrice,
                FinalPrice = originalPrice,
                AppliedDiscount = null
            };

            if (originalPrice <= 0) return result;

            Rabat? best = null;
            decimal bestPrice = originalPrice;

            foreach (var rabat in alleRabatter)
            {
                // Only the chosen discount (if any)
                if (!string.IsNullOrEmpty(valgtRabatCode) &&
                    !string.Equals(rabat.Code, valgtRabatCode, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // If loyalty tier is required
                if (!string.IsNullOrEmpty(rabat.RequiredLoyaltyTier) &&
                    !string.Equals(kunde?.LoyaltyTier, rabat.RequiredLoyaltyTier, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var priceWithDiscount = originalPrice * (1 - rabat.Percentage);
                if (priceWithDiscount < bestPrice)
                {
                    bestPrice = priceWithDiscount;
                    best = rabat;
                }
            }

            if (best != null)
            {
                result.FinalPrice = bestPrice;
                result.AppliedDiscount = best;
            }

            return result;
        }

    }
}