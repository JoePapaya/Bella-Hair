using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BellaHair.Domain.Entities;

public class Booking
{
    public int BookingId { get; set; }

    public int KundeId { get; set; }
    public int BehandlingId { get; set; }
    public int MedarbejderId { get; set; }

    public DateTime Tidspunkt { get; set; }   // start tidspunkt
    public int Varighed { get; set; }         // minutter
    public DateTime Start => Tidspunkt;
    public DateTime End => Tidspunkt.AddMinutes(Varighed);

<<<<<<< HEAD
=======
    public DateTime Start => Tidspunkt;

    public DateTime End => Tidspunkt.AddMinutes(Varighed);
    
>>>>>>> 480717a31ff5de04dbcc12f2b2f81e1084c3c622
    public BookingStatus Status { get; set; }
    public string? ValgtRabat { get; set; }

    // Computed properties for kalenderen (NOT mapped to DB)

    // Navigation properties (hvis du bruger dem i UI)
    public Kunde? Kunde { get; set; }
    public Medarbejder? Medarbejder { get; set; }
    public Behandling? Behandling { get; set; }


}


public enum BookingStatus
{
    Bekræftet,
    Afventer,
    Gennemført
}

