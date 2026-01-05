using BellaHair.Domain.Entities;

public class Faktura

//“Fakturaen er en entity, men dens felter er gemt som snapshot-data
//(immutable værdier) inspireret af value object-tankegangen.”
{
    public int FakturaId { get; set; }
    public int KundeId { get; set; }
    public int BookingId { get; set; }

    // Kunde-snapshot
    public string KundeNavn { get; set; } = string.Empty;
    public string? KundeEmail { get; set; }
    public string? KundeTelefon { get; set; }

    // Behandling / medarbejder
    public string? BehandlingNavn { get; set; }
    public string? MedarbejderNavn { get; set; }
    
    //tidspunkt snapshot
    public DateTime BookingTidspunkt { get; set; }
    public DateTime FakturaDato { get; set; }

    // Firma / privat
    public bool ErFirmafaktura { get; set; }
    public string? Firmanavn { get; set; }
    public string? Cvr { get; set; }

    // Beløb
    public decimal Beløb { get; set; }
    public decimal RabatBeløb { get; set; }
    public decimal TotalBeløb { get; set; }
    public string? RabatTekst { get; set; }
}