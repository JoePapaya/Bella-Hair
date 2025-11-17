using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class Behandling
{
    public int BehandlingId { get; set; }
    public string Navn { get; set; } = string.Empty;
    public decimal Pris { get; set; }
    public int VarighedMinutter { get; set; }
}

