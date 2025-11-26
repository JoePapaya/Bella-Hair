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
        // Brug booking-datoen hvis vi får den, ellers "i dag"
        var dato = bookingDate?.Date ?? DateTime.Today;

        // Start med alle rabatter fra databasen
        var alleRabatter = _dataService.Rabatter.AsEnumerable();

        // Kun aktive rabatter
        alleRabatter = alleRabatter.Where(r => r.Aktiv);

        // Respekter kampagneperioder
        alleRabatter = alleRabatter.Where(r => r.IsWithinCampaignPeriod(dato));

        // Respekter evt. loyalitetskrav
        alleRabatter = alleRabatter.Where(r =>
            string.IsNullOrWhiteSpace(r.RequiredLoyaltyTier) ||
            string.Equals(
                kunde?.LoyaltyTier,
                r.RequiredLoyaltyTier,
                StringComparison.OrdinalIgnoreCase));

        // Lad din eksisterende DiscountCalc-logik finde den bedste
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

        // Kun aktive rabatter
        rabatter = rabatter.Where(r => r.Aktiv);

        // Respekter kampagneperioder ift. "i dag"
        rabatter = rabatter.Where(r => r.IsWithinCampaignPeriod(dato));

        // Loyalitetskrav
        rabatter = rabatter.Where(r =>
            string.IsNullOrWhiteSpace(r.RequiredLoyaltyTier) ||
            string.Equals(
                kunde?.LoyaltyTier,
                r.RequiredLoyaltyTier,
                StringComparison.OrdinalIgnoreCase));

        return rabatter.OrderBy(r => r.Navn);
    }
}
