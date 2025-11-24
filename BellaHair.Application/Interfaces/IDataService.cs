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

        // ---------- Medarbejder ----------

        Task AddMedarbejderAsync(Medarbejder medarbejder);
        Task UpdateMedarbejderAsync(Medarbejder medarbejder);
        Task DeleteMedarbejderAsync(int medarbejderId);
        Task<Medarbejder?> GetMedarbejderAsync(int medarbejderId);

        // ---------- Kunde ----------
        Task AddKundeAsync(Kunde kunde);
        Task UpdateKundeAsync(Kunde kunde);
        Task DeleteKundeAsync(int kundeId);
        Task<Kunde?> GetKundeAsync(int kundeId);

        // ---------- Behandling ----------
        Task AddBehandlingAsync(Behandling behandling);
        Task UpdateBehandlingAsync(Behandling behandling);
        Task DeleteBehandlingAsync(int behandlingId);
        Task<Behandling?> GetBehandlingAsync(int behandlingId);

        // ---------- Booking ----------
        Task<Behandling?> GetBehandlingAsync(string navn);
        Task<Booking?> GetBookingAsync(int bookingId);
        Task<Booking> AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);

        // ---------- Rabat ----------
        Task<Rabat> AddRabatAsync(Rabat rabat);
        Task<Rabat> UpdateRabatAsync(Rabat rabat);
        Task DeleteRabatAsync(int id);
        Task<Rabat?> GetRabatAsync(int id);



    }
}
