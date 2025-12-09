using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;
using BellaHair.Domain.Enums;


namespace BellaHair.Application.Services
{
    public class BookingValidationService : IBookingValidationService
    {
        private readonly IDataService _data;

        public BookingValidationService(IDataService data)
        {
            _data = data;
        }

        public Task ValidateAsync(Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            if (booking.Varighed <= 0)
                throw new InvalidOperationException("Varighed skal være større end 0.");

            var now = DateTime.Now;

            // --- Regler for tid ift. status ---

            if (booking.Status == BookingStatus.Kommende && booking.Tidspunkt < now)
            {
                throw new InvalidOperationException("Kommende bookinger skal ligge i fremtiden.");
            }

            if (booking.Status == BookingStatus.Gennemført && booking.Tidspunkt > now)
            {
                throw new InvalidOperationException("Gennemførte bookinger skal ligge i fortiden.");
            }

            // Find eksisterende bookinger for samme medarbejder
            var eksisterende = _data.Bookinger
                .Where(b => b.MedarbejderId == booking.MedarbejderId &&
                            b.BookingId != booking.BookingId);

            foreach (var b in eksisterende)
            {
                bool overlapper =
                    booking.Start < b.End &&
                    booking.End > b.Start;

                if (overlapper)
                    throw new InvalidOperationException("Der findes allerede en booking i dette tidsinterval.");
            }

            if (booking.KundeId != 0)
            {
                var eksisterendeKundeBookinger = _data.Bookinger
                    .Where(b => b.KundeId == booking.KundeId &&
                                b.BookingId != booking.BookingId);

                foreach (var b in eksisterendeKundeBookinger)
                {
                    bool overlapper =
                        booking.Start < b.End &&
                        booking.End > b.Start;

                    if (overlapper)
                        throw new InvalidOperationException(
                            $"Kunden har allerede en anden booking fra {b.Start:t} til {b.End:t}.");
                }
            }

            return Task.CompletedTask;
        }
    }
}
