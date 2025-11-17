using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class Booking
{
    public int BookingId { get; set; }

    public int KundeId { get; set; }
    public int BehandlingId { get; set; }
    public int MedarbejderId { get; set; }

    public DateTime Tidspunkt { get; set; }
    public int Varighed { get; set; }

    // Shown as text in the badge
    public BookingStatus Status { get; set; }

    // Passed to DiscountCalc as booking.ValgtRabat
    public string? ValgtRabat { get; set; }
}

public enum BookingStatus
{
    Bekræftet,
    Afventer,
    Gennemført
}

