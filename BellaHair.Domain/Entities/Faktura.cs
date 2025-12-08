using System;

namespace BellaHair.Domain.Entities;

public class Faktura
{
    public int FakturaId { get; set; }

    // Relationer (bruges kun til historik / navigation)
    public int KundeId { get; set; }
    public int BookingId { get; set; }

    // 🔹 SNAPSHOT AF KUNDE – bruges til visning på faktura
    public string KundeNavn { get; set; } = string.Empty;
    public string? KundeEmail { get; set; }
    public string? KundeTelefon { get; set; }

    // Firma / privat
    public string? Firmanavn { get; set; }
    public bool ErFirmafaktura { get; set; }
    public string? Cvr { get; set; }

    public DateTime FakturaDato { get; set; }

    // Beløb
    public decimal Beløb { get; set; }        // pris før rabat
    public decimal RabatBeløb { get; set; }   // rabat i kroner
    public decimal TotalBeløb { get; set; }   // pris efter rabat

    // Hvad var rabatten? (tekst du vil vise på faktura)
    public string? RabatTekst { get; set; }

    // Navigation (valgfrit, men rart til Include)
    public Kunde? Kunde { get; set; }
    public Booking? Booking { get; set; }
}