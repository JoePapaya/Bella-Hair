using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        // 1) Kun HER validerer vi booking
        await _validator.ValidateAsync(booking);

        // 2) Gem booking
        var savedBooking = await _data.AddBookingAsync(booking);

        // 3) Lav faktura (ingen ekstra validation her)
        await _data.CreateFakturaAsync(savedBooking);

        return savedBooking;
    }

    public async Task UpdateAsync(Booking booking)
    {
        await _validator.ValidateAsync(booking);
        await _data.UpdateBookingAsync(booking);
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _data.DeleteBookingAsync(id);
        }
        catch (DbUpdateException ex)
        {
            var inner = ex.InnerException?.Message ?? ex.Message;
            throw new Exception(inner, ex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
        }
    }




}
