using BellaHair.Domain.Entities;

namespace BellaHair.Application.Interfaces;

public interface ILoyaltyService
{
    Task OpdaterLoyaltyTierAsync(Kunde kunde);
    Task HandleBookingCompletedAsync(int kundeId);
    Task HandleBookingDeletedAsync(int kundeId);

}
