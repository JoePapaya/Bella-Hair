using BellaHair.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

//“Vi nåede ikke at lave et separat KundeApplicationService/validation-lag,
//så pt kan man oprette kunder med tomme strings. Med mere tid ville vi have
//håndhævet ‘Navn er påkrævet’ i Application-laget (KundeValidationService) og evt. UI-validering.”


//*“Vi overvejede at modellere Kunde med arv, hvor PrivatKunde og FirmaKunde arver fra en fælles
//Kunde-baseklasse.Det ville have givet en mere objektorienteret model med tydeligere adskillelse
//af ansvar. Vi valgte dog en enklere løsning med én Kunde-entity og et KundeType-enum, fordi
//forskellene mellem kunde-typerne er begrænsede,og fordi EF Core inheritance ville have øget
//kompleksiteten i mapping, queries og migrations.På grund af tidsbegrænsning valgte vi den
//mere pragmatiske løsning.”*

//“Hvis firmakunder og privatkunder havde haft markant forskellig adfærd, mange forskellige
//felter eller forskellige forretningsregler, ville arv være mere hensigtsmæssig.”