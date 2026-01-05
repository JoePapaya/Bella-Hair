using BellaHair.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BellaHair.Domain.Entities;

public class Booking
{
    // Primary Key
    // Bruges af Entity Framework til entydigt at identificere en booking
    public int BookingId { get; set; }

    // Foreign Keys
    // Disse værdier gemmes i databasen og skaber relationer til andre tabeller
    public int KundeId { get; set; }
    public int BehandlingId { get; set; }
    public int MedarbejderId { get; set; }

    // Starttidspunkt for bookingen
    // Bruges som grundlag for både kalender og beregninger
    public DateTime Tidspunkt { get; set; }

    // Varighed af bookingen i minutter
    // Bruges til at beregne sluttidspunktet
    public int Varighed { get; set; }

    // Beregnet property
    // Returnerer altid starttidspunktet
    // Bruges for læsbarhed i kode og UI
    public DateTime Start => Tidspunkt;

    // Beregnet property
    // Sluttidspunkt beregnes dynamisk ud fra starttidspunkt + varighed
    // Gemmes ikke i databasen
    public DateTime End => Tidspunkt.AddMinutes(Varighed);

    // Status for bookingen (fx Oprettet, Bekræftet, Aflyst)
    // Enum gør tilladte værdier tydelige og typesikre
    public BookingStatus Status { get; set; }

    // Valgt rabatkode ved booking
    // Nullable fordi en booking godt kan være uden rabat
    public string? ValgtRabat { get; set; }

    // Navigation properties
    // Bruges til at navigere fra Booking til relaterede entiteter
    // Disse bliver typisk brugt i UI og domænelogik
    // De er nullable, fordi EF Core ikke altid loader dem automatisk
    public Kunde? Kunde { get; set; }
    public Medarbejder? Medarbejder { get; set; }
    public Behandling? Behandling { get; set; }

    //Navigation properties markeres som nullable, fordi Entity Framework
    //ikke altid indlæser relaterede entiteter automatisk, og de kan derfor være null.

    //Uden navigation properties ville Booking kun indeholde foreign keys, som blot er
    //ID’er. For at få de relaterede objekter skulle man derfor lave manuelle opslag i
    //databasen. Navigation properties gør det muligt for Entity Framework at håndtere
    //relationerne objektorienteret og reducere behovet for ekstra queries.
}