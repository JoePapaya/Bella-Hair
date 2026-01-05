using BellaHair.Domain.Entities;
using BellaHair.Domain.Services.DiscountStrategies;

public class CampaignDiscountStrategy : IDiscountStrategy
{
    private readonly Rabat _rabat;

    public CampaignDiscountStrategy(Rabat rabat)
    {
        _rabat = rabat;
    }

    //The Rabat entity is injected through the constructor
    //and stored in a private field so the strategy can access
    //the discount rules throughout its lifetime.


    public bool IsAllowedFor(Kunde? kunde, decimal originalPrice, DateTime dato)
    {
        if (!_rabat.StartDato.HasValue || !_rabat.SlutDato.HasValue)
            return false;

        if (dato < _rabat.StartDato.Value || dato > _rabat.SlutDato.Value)
            return false;

        return true;
    }

    public decimal Apply(decimal originalPrice)
        => _rabat.Apply(originalPrice);

    public bool IsKampagne => true;

    public string Navn => _rabat.Navn;

    public Rabat RabatObjekt => _rabat;


}
