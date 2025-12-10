using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Services;
using BellaHair.Domain.Services.DiscountStrategies;

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
    string? valgtRabatCode,
    DateTime? bookingDate = null)
    {
        var dato = bookingDate?.Date ?? DateTime.Today;

        var rabatter = _dataService.Rabatter
            .Where(r => r.Aktiv)
            .ToList();

        return DiscountCalc.CalculateBestDiscount(
            originalPrice,
            kunde,
            rabatter,
            dato
        );
    }




    public IEnumerable<Rabat> GetTilgængeligeRabatterForKunde(Kunde? kunde)
    {
        var dato = DateTime.Today;

        var rabatter = _dataService.Rabatter
            .Where(r => r.Aktiv)
            .Where(r => r.IsWithinCampaignPeriod(dato))
            .Where(r => r.IsEligibleFor(kunde))
            .OrderBy(r => r.Navn);

        return rabatter;
    }

}
