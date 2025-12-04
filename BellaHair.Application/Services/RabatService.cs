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
        string? valgtRabatCode,
        DateTime? bookingDate = null)
    {
        var dato = bookingDate?.Date ?? DateTime.Today;

        var alleRabatter = _dataService.Rabatter.AsEnumerable();

        // Kun aktive
        alleRabatter = alleRabatter.Where(r => r.Aktiv);

        // Respekter kampagneperioder
        alleRabatter = alleRabatter.Where(r => r.IsWithinCampaignPeriod(dato));

        // 🔐 NYT: filtrér væk stamkunde-rabatter som kunden IKKE har tier til
        alleRabatter = alleRabatter.Where(r => DiscountCalc.IsRabatAllowedForKunde(r, kunde));

        return DiscountCalc.CalculateBestDiscount(
            originalPrice,
            kunde,
            valgtRabatCode,
            alleRabatter.ToList());
    }


    public IEnumerable<Rabat> GetTilgængeligeRabatterForKunde(Kunde? kunde)
    {
        var dato = DateTime.Today;

        var rabatter = _dataService.Rabatter.AsEnumerable();

        rabatter = rabatter.Where(r => r.Aktiv);
        rabatter = rabatter.Where(r => r.IsWithinCampaignPeriod(dato));
        rabatter = rabatter.Where(r => r.IsEligibleFor(kunde));

        return rabatter.OrderBy(r => r.Navn);
    }
}
