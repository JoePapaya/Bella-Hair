using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Entities;

namespace BellaHair.Application.Interfaces
{
    public interface IDataService
    {
        IList<Booking> Bookinger { get; }
        IList<Kunde> Kunder { get; }
        IList<Behandling> Behandlinger { get; }
        IList<Medarbejder> Medarbejdere { get; }
        IList<Rabat> Rabatter { get; }

        Task DeleteBookingAsync(int bookingId);

    }
}
