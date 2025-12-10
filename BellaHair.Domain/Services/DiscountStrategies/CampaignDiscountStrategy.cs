using BellaHair.Domain.Entities;
using BellaHair.Domain.Services.DiscountStrategies;

public class CampaignDiscountStrategy : IDiscountStrategy
{
    private readonly Rabat _rabat;

    public CampaignDiscountStrategy(Rabat rabat)
    {
        _rabat = rabat;
    }

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
