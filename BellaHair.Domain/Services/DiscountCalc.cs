using BellaHair.Domain.Entities;
using BellaHair.Domain.Services.DiscountStrategies;

namespace BellaHair.Domain.Services
{
    public static class DiscountCalc
    {
        private static readonly object _syncLock = new object();

        public static bool IsRabatAllowedForKunde(Kunde? kunde, Rabat rabat)
        {
            return rabat.IsEligibleFor(kunde);
        }
        public static DiscountResult CalculateBestDiscount(
        decimal originalPrice,
        Kunde? kunde,
        IEnumerable<Rabat> rabatter,
        DateTime dato)
        {
            lock (_syncLock)
            {
                var strategies = rabatter
                    .Select(DiscountStrategyFactory.Create)
                    .ToList();

                decimal bestFinal = originalPrice;
                IDiscountStrategy? best = null;

                foreach (var strategy in strategies)
                {
                    if (!strategy.IsAllowedFor(kunde, originalPrice, dato))
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
