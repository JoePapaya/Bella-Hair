using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;

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

            return Task.CompletedTask;
        }
    }
}
