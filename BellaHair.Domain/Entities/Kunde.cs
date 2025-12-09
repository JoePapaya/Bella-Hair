using BellaHair.Domain.Enums;
using System;

namespace BellaHair.Domain.Entities;

public class Kunde
{
    public int KundeId { get; set; }
    public string Navn { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Postnr { get; set; } = string.Empty;
    public string By { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public DateOnly Fødselsdag { get; set; }

    public KundeType KundeType { get; set; } = KundeType.Privat;

    public string? Firmanavn { get; set; }
    public string? Cvr { get; set; }
    public int Points { get; set; } = 0;
    public int BesøgAntal { get; set; } = 0;

    // Bronze / Sølv / Guld / None – som tekst
    public LoyaltyTier LoyaltyTier { get; set; } = LoyaltyTier.None;
}
