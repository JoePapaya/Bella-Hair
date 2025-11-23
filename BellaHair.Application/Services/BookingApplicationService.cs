using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;

namespace BellaHair.Application.Services;

public class BookingApplicationService : IBookingApplicationService
{
    private readonly IDataService _data;

    public BookingApplicationService(IDataService data)
    {
        _data = data;
    }

    public Task<IList<Booking>> GetAllAsync()
        => Task.FromResult(_data.Bookinger);

    public Task<Booking?> GetByIdAsync(int id)
        => _data.GetBookingAsync(id);

    public Task<Booking> CreateAsync(Booking booking)
        => _data.AddBookingAsync(booking);

    public Task UpdateAsync(Booking booking)
        => _data.UpdateBookingAsync(booking);

    public Task DeleteAsync(int id)
        => _data.DeleteBookingAsync(id);
}
