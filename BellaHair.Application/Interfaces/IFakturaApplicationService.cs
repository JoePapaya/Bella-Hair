using BellaHair.Domain.Entities;

namespace BellaHair.Application.Interfaces;

public interface IFakturaApplicationService
{
    Task<Faktura?> GetForBookingAsync(int bookingId);
    Task<Faktura> EnsureForBookingAsync(Booking booking);
}

//“Spørgsmålstegnet betyder, at returtypen kan være null.
//Task repræsenterer et asynkront arbejde, der returnerer en værdi senere.”