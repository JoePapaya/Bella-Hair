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

    // Bronze/Sølv/Guld – used in the page
    public string? LoyaltyTier { get; set; }
}

