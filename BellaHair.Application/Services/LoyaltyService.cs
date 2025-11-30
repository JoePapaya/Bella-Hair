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

    public string BeregnLoyaltyTierForKunde(Kunde kunde)
    {
        var antalGennemførte = _dataService.Bookinger
            .Count(b => b.KundeId == kunde.KundeId &&
                        b.Status == BookingStatus.Gennemført);

        // Opgave-krav:
        // 5–9  = Bronze
        // 10–19 = Sølv
        // 20+   = Guld

        if (antalGennemførte >= 20) return "Guld";
        if (antalGennemførte >= 10) return "Sølv";
        if (antalGennemførte >= 5) return "Bronze";

        return "None";
    }

    public async Task OpdaterLoyaltyTierAsync(Kunde kunde)
    {
        var nyTier = BeregnLoyaltyTierForKunde(kunde);

        if (!string.Equals(kunde.LoyaltyTier, nyTier, StringComparison.OrdinalIgnoreCase))
        {
            kunde.LoyaltyTier = nyTier;
            await _dataService.UpdateKundeAsync(kunde);
        }
    }
}
