using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;

namespace BellaHair.Application.Services;

public class BookingApplicationService : IBookingApplicationService
{
    private readonly IDataService _data;
    private readonly IBookingValidationService _validator;

    public BookingApplicationService(IDataService data, IBookingValidationService validator)
    {
        _data = data;
        _validator = validator;
    }

    public Task<IList<Booking>> GetAllAsync()
        => Task.FromResult(_data.Bookinger);

    public Task<Booking?> GetByIdAsync(int id)
        => _data.GetBookingAsync(id);

    public async Task<Booking> CreateAsync(Booking booking)
    {
        await _validator.ValidateAsync(booking);
        return await _data.AddBookingAsync(booking);
    }

    public async Task UpdateAsync(Booking booking)
    {
        await _validator.ValidateAsync(booking);
        await _data.UpdateBookingAsync(booking);
    }

    public Task DeleteAsync(int id)
        => _data.DeleteBookingAsync(id);
}
