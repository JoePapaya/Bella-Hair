using BellaHair.Domain.Enums; 

namespace BellaHair.Domain.Entities;

public class Rabat
{
    public int RabatId { get; set; }

    public string Navn { get; set; } = string.Empty;

    public decimal? Percentage { get; set; }
    public decimal? FixedAmount { get; set; }

    // Stamkunde-rabat: kræver Bronze,Silver, eller Gold
    public LoyaltyTier? RequiredLoyaltyTier { get; set; }

    //“None i LoyaltyTier repræsenterer, at en kunde endnu ikke har
    //opnået et loyalitetsniveau. Den adskilles fra null, som i
    //rabatsammenhæng bruges til at angive, at der ikke er et krav.”


    public bool Aktiv { get; set; } = true;
    public string Description { get; set; } = string.Empty;

    // Kampagne
    public bool IsKampagne { get; set; }
    public DateTime? StartDato { get; set; }
    public DateTime? SlutDato { get; set; }

    // Minimum ordrebeløb for at rabatten gælder
    public decimal? MinimumBeløb { get; set; }




    // Tjekker om rabatten gælder på den givne dato
    public bool IsWithinCampaignPeriod(DateTime dato)
    {
        // Ikke en kampagne → gælder altid
        if (!IsKampagne)
            return true;

        // Kampagne uden gyldige datoer → gælder ikke
        if (StartDato is null || SlutDato is null)
            return false;

        // Datoen skal ligge mellem start- og slutdato (inkl.)
        return dato >= StartDato.Value.Date &&
               dato <= SlutDato.Value.Date;
    }


    // Tjekker om en bestemt kunde må bruge rabatten
    public bool IsEligibleFor(Kunde? kunde)
    {
        // Kampagnerabat → alle kan bruge den
        if (IsKampagne)
            return true;

        // Ingen loyalty-krav → alle kan bruge den
        if (RequiredLoyaltyTier is null)
            return true;

        // Stamkunderabat kræver, at der findes en kunde
        if (kunde is null)
            return false;

        // Kunden skal have samme eller højere loyalty-tier end kravet
        return kunde.LoyaltyTier >= RequiredLoyaltyTier.Value;
    }


    // Beregner prisen efter rabat
    public decimal Apply(decimal originalPrice)
    {
        // Procentrabat (fx 0.10 = 10%)
        if (Percentage is > 0)
        {
            return originalPrice * (1 - Percentage.Value);
        }

        // Fast beløbsrabat (fx 50 kr) – prisen må ikke blive negativ
        if (FixedAmount is > 0)
        {
            return Math.Max(0, originalPrice - FixedAmount.Value);
        }

        // Ingen rabat → returner original pris
        return originalPrice;
    }


    //Metoderne er placeret i Rabat, fordi de kun bruger rabattens egne data og
    //beskriver forretningsregler, som naturligt hører til rabatten, fx hvornår
    //den er gyldig, hvem der må bruge den, og hvordan prisen beregnes. Det følger
    //DDD-princippet om en rig domænemodel og overholder Single Responsibility Principle.
}
