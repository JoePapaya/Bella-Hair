using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class Behandling
{
    public int BehandlingId { get; set; }

    // string.Empty sikrer, at Navn aldrig er null
    // Det forhindrer NullReferenceExceptions og compiler warnings
    // Især vigtigt fordi Entity Framework opretter objekter uden constructor
    public string Navn { get; set; } = string.Empty;
    public decimal Pris { get; set; }
    public int VarighedMinutter { get; set; }
    public string Type { get; set; } = string.Empty;
}