using System;

namespace BellaHair.Domain.Entities;

public class Rabat
{
    public int RabatId { get; set; }

    public string Navn { get; set; } = string.Empty;

    // Kode kunden kan skrive (valgfri)
    public string? Code { get; set; }

    // Procent-rabat gemt som 0–1 (fx 0.10 = 10%).
    public decimal? Percentage { get; set; }

    // Fast beløb i kr. Bruges hvis sat og Percentage ikke er sat.
    public decimal? FixedAmount { get; set; }

    // Hvilket loyalitetsniveau der kræves (Bronze/Sølv/Guld) – som tekst
    public string? RequiredLoyaltyTier { get; set; }

    // Aktiver/deaktiver rabatten
    public bool Aktiv { get; set; } = true;
    public string Description { get; set; } = string.Empty;

    // Kampagne
    public bool IsKampagne { get; set; }         // true = kampagnerabat
    public DateTime? StartDato { get; set; }
    public DateTime? SlutDato { get; set; }

    // Valgfrit: minimum totalbeløb
    public decimal? MinimumBeløb { get; set; }

    // ------- Hjælpere -------

    public bool IsWithinCampaignPeriod(DateTime dato)
    {
        if (!IsKampagne)
            return true;

        if (StartDato.HasValue && dato < StartDato.Value.Date)
            return false;

        if (SlutDato.HasValue && dato > SlutDato.Value.Date)
            return false;

        return true;
    }

    public decimal Apply(decimal originalPrice)
    {
        // Brug procent KUN hvis den faktisk er > 0
        if (Percentage.HasValue && Percentage.Value > 0)
        {
            // Percentage gemmes som 0.10 for 10%
            return originalPrice * (1 - Percentage.Value);
        }

        // Ellers prøver vi med fast beløb
        if (FixedAmount.HasValue && FixedAmount.Value > 0)
        {
            return Math.Max(0, originalPrice - FixedAmount.Value);
        }

        // Hvis hverken procent eller fast beløb giver mening → ingen rabat
        return originalPrice;
    }
}
