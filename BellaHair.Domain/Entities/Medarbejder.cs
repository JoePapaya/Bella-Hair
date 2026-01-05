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

//“Vi har validering på booking via en separat BookingValidationService, men vi
//nåede ikke at lave samme mønster for Medarbejder. Derfor kan man i dag oprette
//en medarbejder med tomme strings, fordi string.Empty kun sikrer ‘ikke-null’ og
//ikke at feltet er udfyldt. Med mere tid ville vi have lavet en MedarbejderValidationService
//i Application-laget og kaldt den fra medarbejderens application service før data gemmes.
//Derudover kunne vi også have tilføjet UI-validering som ekstra lag.”