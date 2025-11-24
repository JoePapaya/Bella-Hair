using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class Rabat
{
    public int RabatId { get; set; }
    public string Navn { get; set; } = string.Empty; // e.g. "STUDIERABAT"
    public string Description { get; set; } = string.Empty;
    public decimal Percentage { get; set; }          // 0.10m = 10 %
    public string? RequiredLoyaltyTier { get; set; } // optional
}
