using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class Kunde
{
    public int KundeId { get; set; }
    public string Navn { get; set; } = string.Empty;
    public string Adresse {  get; set; } = string.Empty;
    public string Postnr { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly Fødselsdag { get; set; }
    public string By { get; set; } = string.Empty;

    public int Points { get; set; } = 0;

    public string Telefon { get; set; } = string.Empty;

    public int BesøgAntal { get; set; } = 0;

    // Bronze/Sølv/Guld – used in the page
    public string? LoyaltyTier { get; set; }
}

