using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;

namespace BellaHair.Application.Services;

public class FakturaApplicationService : IFakturaApplicationService
{
    private readonly IDataService _data;

    public FakturaApplicationService(IDataService data)
    {
        _data = data;
    }

    public Task<Faktura?> GetForBookingAsync(int bookingId)
        => _data.GetFakturaForBookingAsync(bookingId);

    public async Task<Faktura> EnsureForBookingAsync(Booking booking)
    {
        var eksisterende = await _data.GetFakturaForBookingAsync(booking.BookingId);
        if (eksisterende is not null)
            return eksisterende;

        return await _data.CreateFakturaAsync(booking);
    }
}
