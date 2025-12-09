using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums; 

namespace BellaHair.Application.Services;

public class LoyaltyService : ILoyaltyService
{
    private readonly IDataService _dataService;

    public LoyaltyService(IDataService dataService)
    {
        _dataService = dataService;
    }

    private LoyaltyTier BeregnLoyaltyTier(int antalGennemførte)
    {
        if (antalGennemførte >= 20) return LoyaltyTier.Gold;
        if (antalGennemførte >= 10) return LoyaltyTier.Silver;
        if (antalGennemførte >= 5) return LoyaltyTier.Bronze;
        return LoyaltyTier.None;
    }


    public async Task OpdaterLoyaltyTierAsync(Kunde kunde)
    {
        var antalGennemførte = await _dataService.GetCompletedBookingsCountForKundeAsync(kunde.KundeId);
        var nyTier = BeregnLoyaltyTier(antalGennemførte);

        if (kunde.LoyaltyTier != nyTier)
        {
            kunde.LoyaltyTier = nyTier;
            await _dataService.UpdateKundeAsync(kunde);
        }
    }


    public async Task HandleBookingCompletedAsync(int kundeId)
    {
        var kunde = await _dataService.GetKundeAsync(kundeId);
        if (kunde is null) return;

        await OpdaterLoyaltyTierAsync(kunde);
    }

    public async Task HandleBookingDeletedAsync(int kundeId)
    {
        var kunde = await _dataService.GetKundeAsync(kundeId);
        if (kunde is null) return;

        await OpdaterLoyaltyTierAsync(kunde);
    }
}
