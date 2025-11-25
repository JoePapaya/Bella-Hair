using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class Faktura
{
    public int FakturaId { get; set; }

    // Relationer
    public int KundeId { get; set; }
    public int BookingId { get; set; }

    // Firma / privat
    public bool ErFirmafaktura { get; set; }
    public string? Firmanavn { get; set; }
    public string? Cvr { get; set; }

    public DateTime FakturaDato { get; set; }

    // Beløb
    public decimal Beløb { get; set; }        // pris før rabat
    public decimal RabatBeløb { get; set; }   // rabat i kroner
    public decimal TotalBeløb { get; set; }   // pris efter rabat

    // Hvad var rabatten? (tekst du vil vise på faktura)
    // fx "Studierabat 10%" eller "Black Friday 20%"
    public string? RabatTekst { get; set; }

    // Navigation (valgfrit, men rart til Include)
    public Kunde? Kunde { get; set; }
    public Booking? Booking { get; set; }
}

