using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Entities;

namespace BellaHair.Application.Interfaces;

public interface IBookingApplicationService
{
    Task<IList<Booking>> GetAllAsync();
    Task<Booking?> GetByIdAsync(int id);
    Task<Booking> CreateAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(int id);
}
