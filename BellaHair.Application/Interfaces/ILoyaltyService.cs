using BellaHair.Domain.Entities;

namespace BellaHair.Application.Interfaces;

public interface ILoyaltyService
{
    LoyaltyTier BeregnLoyaltyTierForKunde(Kunde kunde);
    Task OpdaterLoyaltyTierAsync(Kunde kunde);
    Task HandleBookingCompletedAsync(int kundeId);
    Task HandleBookingDeletedAsync(int kundeId);

}
