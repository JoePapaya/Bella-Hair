using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Services;

namespace BellaHair.Application.Services;

public class RabatService : IRabatService
{
    private readonly IDataService _dataService;

    public RabatService(IDataService dataService)
    {
        _dataService = dataService;
    }

    public DiscountResult BeregnBedsteRabat(
        decimal originalPrice,
        Kunde? kunde,
        string? valgtRabatCode)
    {
        var alleRabatter = _dataService.Rabatter;

        return DiscountCalc.CalculateBestDiscount(
            originalPrice,
            kunde,
            valgtRabatCode,
            alleRabatter);
    }

    public IEnumerable<Rabat> GetTilgængeligeRabatterForKunde(Kunde? kunde)
    {
        var rabatter = _dataService.Rabatter.AsEnumerable();

        // Hvis rabatten kræver tier -> kun hvis kunden matcher
        rabatter = rabatter.Where(r =>
            string.IsNullOrWhiteSpace(r.RequiredLoyaltyTier) ||
            string.Equals(kunde?.LoyaltyTier, r.RequiredLoyaltyTier, StringComparison.OrdinalIgnoreCase));

        return rabatter.OrderBy(r => r.Navn);
    }
}
