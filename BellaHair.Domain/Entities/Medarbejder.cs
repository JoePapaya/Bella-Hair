using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class Medarbejder
{
    public int MedarbejderId { get; set; }
    public string Navn { get; set; } = string.Empty;
}
