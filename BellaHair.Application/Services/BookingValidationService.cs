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

        //“Vi definerer et interface, IBookingValidationService, som
        //beskriver hvilke valideringsoperationer der findes. Det gør
        //det muligt at dependency injecte servicen alle steder, hvor
        //validering er nødvendig. BookingValidationService implementerer
        //interfacet og får IDataService injected via constructoren.
        //De private readonly fields er referencer til disse afhængigheder.
        //Klassen opretter ikke selv sine afhængigheder, men får dem leveret
        //af DI-containeren.”

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
          
            //“Find alle bookinger for den samme medarbejder,
            //undtagen den booking, vi selv er i gang med.”

            foreach (var b in eksisterende)
            {
                bool overlapper =
                    booking.Start < b.End &&
                    booking.End > b.Start;

                if (overlapper)
                    throw new InvalidOperationException("Der findes allerede en booking i dette tidsinterval.");
            }

            //“Koden gennemløber eksisterende bookinger og tjekker,
            //om tidsintervallet overlapper med den nye booking.
            //Hvis der findes overlap, kastes en exception for at
            //forhindre dobbeltbooking.”

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


            //“Koden tjekker, om en kunde allerede har en anden booking,
            //der overlapper i tid. Hvis der findes overlap, kastes en
            //exception for at forhindre dobbeltbooking.”


            return Task.CompletedTask;
        }
    }
}
