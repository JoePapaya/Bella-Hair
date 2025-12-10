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

        public bool IsAllowedFor(Kunde? kunde, decimal originalPrice, DateTime dato)
        {
            // Ikke kampagne → tjek eligibility
            if (!_rabat.IsEligibleFor(kunde))
                return false;

            if (_rabat.MinimumBeløb.HasValue && originalPrice < _rabat.MinimumBeløb.Value)
                return false;

            return true;
        }

        public decimal Apply(decimal originalPrice)
        {
            return _rabat.Apply(originalPrice);
        }

        public bool IsKampagne => false;
        public string Navn => _rabat.Navn;
        public Rabat RabatObjekt => _rabat;
    }
}
