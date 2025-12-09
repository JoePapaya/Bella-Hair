using BellaHair.Domain.Entities;

namespace BellaHair.Application.Interfaces;

public interface IFakturaApplicationService
{
    Task<Faktura?> GetForBookingAsync(int bookingId);
    Task<Faktura> EnsureForBookingAsync(Booking booking);
}
