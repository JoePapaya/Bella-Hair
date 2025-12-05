using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Services;

namespace BellaHair.Domain.Services.DiscountStrategies
{
    public static class DiscountStrategyFactory
    {
        public static IDiscountStrategy Create(Rabat rabat)
        {
            if (rabat.Navn.StartsWith("Stamkunde", StringComparison.OrdinalIgnoreCase))
                return new LoyaltyDiscountStrategy(rabat);

            return new CampaignDiscountStrategy(rabat);

        }
    }
}
