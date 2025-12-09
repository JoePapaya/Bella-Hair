using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums; // Eller hvor din LoyaltyTier enum ligger

namespace BellaHair.Application.Services;

public class LoyaltyService : ILoyaltyService
{
    private readonly IDataService _dataService;

    public LoyaltyService(IDataService dataService)
    {
        _dataService = dataService;
    }

    public LoyaltyTier BeregnLoyaltyTierForKunde(Kunde kunde)
    {
        var antalGennemførte = _dataService.Bookinger
            .Count(b => b.KundeId == kunde.KundeId &&
                        b.Status == BookingStatus.Gennemført);

        if (antalGennemførte >= 20) return LoyaltyTier.Gold;
        if (antalGennemførte >= 10) return LoyaltyTier.Silver;
        if (antalGennemførte >= 5) return LoyaltyTier.Bronze;

        return LoyaltyTier.None;
    }

    public async Task OpdaterLoyaltyTierAsync(Kunde kunde)
    {
        var nyTier = BeregnLoyaltyTierForKunde(kunde);

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
