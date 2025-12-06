using BellaHair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Services.DiscountStrategies
{
    public class CampaignDiscountStrategy : IDiscountStrategy
    {
        private readonly Rabat _rabat;

        public CampaignDiscountStrategy(Rabat rabat)
        {
            _rabat = rabat;
        }

        public bool IsAllowedFor(Kunde? kunde) => true;

        public decimal Apply(decimal originalPrice)
        {
            return _rabat.Apply(originalPrice);
        }

        public bool IsKampagne => _rabat.IsKampagne;
        public string Navn => _rabat.Navn;
        public Rabat RabatObjekt => _rabat;

    }
}
