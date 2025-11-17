using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class Medarbejder
{
    public int MedarbejderId { get; set; }
    public bool ErFreelancer { get; set; }
    public string Navn { get; set; } = string.Empty;
    public List<string> Kompetencer { get; set; } = new List<string>();
}

