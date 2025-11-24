using BellaHair.Domain.Entities;

namespace BellaHair.Application.Interfaces;

public interface ILoyaltyService
{
    string BeregnLoyaltyTierForKunde(Kunde kunde);

    Task OpdaterLoyaltyTierAsync(Kunde kunde);
}
