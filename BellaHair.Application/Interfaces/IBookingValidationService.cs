using BellaHair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Application.Interfaces
{
    public interface IBookingValidationService
    {
        Task ValidateAsync(Booking booking);
    }

}
