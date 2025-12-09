using BellaHair.Domain.Entities;

namespace BellaHair.Domain.Services.DiscountStrategies
{
    public class LoyaltyDiscountStrategy : IDiscountStrategy
    {
        private readonly Rabat _rabat;

        public LoyaltyDiscountStrategy(Rabat rabat)
        {
            _rabat = rabat;
        }

        public bool IsAllowedFor(Kunde? kunde)
        {
         
            return _rabat.IsEligibleFor(kunde);
        }

        public decimal Apply(decimal originalPrice)
        {
            return _rabat.Apply(originalPrice);
        }

        public bool IsKampagne => _rabat.IsKampagne;
        public string Navn => _rabat.Navn;
        public Rabat RabatObjekt => _rabat;
    }
}
