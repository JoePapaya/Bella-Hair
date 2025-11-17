using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Application.Interfaces;
using BellaHair.Domain.Entities;

namespace BellaHair.Infrastructure
{
    public class InMemoryDataService : IDataService
    {
        public IList<Booking> Bookinger { get; } = new List<Booking>();
        public IList<Kunde> Kunder { get; } = new List<Kunde>();
        public IList<Behandling> Behandlinger { get; } = new List<Behandling>();
        public IList<Medarbejder> Medarbejdere { get; } = new List<Medarbejder>();
        public IList<Rabat> Rabatter { get; } = new List<Rabat>();

        public InMemoryDataService()
        {
            Seed();
        }

        private void Seed()
        {
            // --- dummy data so the page has something to show ---

            var kunde1 = new Kunde { KundeId = 1, Navn = "Anna Jensen", LoyaltyTier = "Guld" };
            var kunde2 = new Kunde { KundeId = 2, Navn = "Mads Nielsen", LoyaltyTier = null };

            Kunder.Add(kunde1);
            Kunder.Add(kunde2);

            var behandling1 = new Behandling { BehandlingId = 1, Navn = "Dameklip", Pris = 450m, VarighedMinutter = 45 };
            var behandling2 = new Behandling { BehandlingId = 2, Navn = "Herreklip", Pris = 350m, VarighedMinutter = 30 };

            Behandlinger.Add(behandling1);
            Behandlinger.Add(behandling2);

            var medarbejder1 = new Medarbejder { MedarbejderId = 1, Navn = "Maria" };
            var medarbejder2 = new Medarbejder { MedarbejderId = 2, Navn = "Sofie" };

            Medarbejdere.Add(medarbejder1);
            Medarbejdere.Add(medarbejder2);

            Rabatter.Add(new Rabat
            {
                RabatId = 1,
                Code = "LOYALTY_GULD",
                Description = "Guld loyalitetsrabat 15%",
                Percentage = 0.15m,
                RequiredLoyaltyTier = "Guld"
            });

            Rabatter.Add(new Rabat
            {
                RabatId = 2,
                Code = "STUDIERABAT",
                Description = "Studierabat 10%",
                Percentage = 0.10m
            });

            Bookinger.Add(new Booking
            {
                BookingId = 1,
                KundeId = 1,
                BehandlingId = 1,
                MedarbejderId = 1,
                Tidspunkt = DateTime.Today.AddHours(10),
                Varighed = 45,
                Status = BookingStatus.Bekræftet,
                ValgtRabat = "LOYALTY_GULD"
            });

            Bookinger.Add(new Booking
            {
                BookingId = 2,
                KundeId = 2,
                BehandlingId = 2,
                MedarbejderId = 2,
                Tidspunkt = DateTime.Today.AddHours(13),
                Varighed = 30,
                Status = BookingStatus.Afventer,
                ValgtRabat = null
            });
        }

        public Task DeleteBookingAsync(int bookingId)
        {
            var booking = Bookinger.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                Bookinger.Remove(booking);
            }

            return Task.CompletedTask;
        }



    }
}
