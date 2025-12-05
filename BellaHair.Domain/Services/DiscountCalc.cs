using BellaHair.Domain.Entities;
using BellaHair.Domain.Services.DiscountStrategies;

namespace BellaHair.Domain.Services
{
    public static class DiscountCalc
    {
        private static readonly object _syncLock = new object();

        public static bool IsRabatAllowedForKunde(Kunde? kunde, Rabat rabat)
        {
            if (kunde == null) return false;
            if (!rabat.Navn.StartsWith("Stamkunde", StringComparison.OrdinalIgnoreCase))
                return true;

            return rabat.Navn.Contains(kunde.LoyaltyTier ?? "", StringComparison.OrdinalIgnoreCase);
        }

        public static DiscountResult CalculateBestDiscount(
            decimal originalPrice,
            Kunde? kunde,
            IEnumerable<Rabat> rabatter)
        {
            lock (_syncLock)
            {
                var strategies = rabatter
                    .Select(r => DiscountStrategyFactory.Create(r))
                    .ToList();

                decimal bestFinal = originalPrice;
                IDiscountStrategy? best = null;

                foreach (var strategy in strategies)
                {
                    if (!strategy.IsAllowedFor(kunde))
                        continue;

                    var final = strategy.Apply(originalPrice);

                    if (final < bestFinal)
                    {
                        bestFinal = final;
                        best = strategy;
                    }
                    else if (final == bestFinal && best != null)
                    {
                        if (!strategy.IsKampagne && best.IsKampagne)
                            best = strategy;
                    }
                }

                return new DiscountResult
                {
                    OriginalPrice = originalPrice,
                    FinalPrice = bestFinal,
                    AppliedDiscount = best?.RabatObjekt
                };
            }
        }
    }
}
