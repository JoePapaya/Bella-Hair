using BellaHair.Domain.Enums; // hvis LoyaltyTier ligger dér

namespace BellaHair.Domain.Entities;

public class Rabat
{
    public int RabatId { get; set; }

    public string Navn { get; set; } = string.Empty;

    public decimal? Percentage { get; set; }
    public decimal? FixedAmount { get; set; }

    // Stamkunde-rabat: kræver Bronze/Silver/Gold
    // null = ingen krav (typisk kampagnerabat eller generel rabat)
    public LoyaltyTier? RequiredLoyaltyTier { get; set; }

    public bool Aktiv { get; set; } = true;
    public string Description { get; set; } = string.Empty;

    // Kampagne
    public bool IsKampagne { get; set; }
    public DateTime? StartDato { get; set; }
    public DateTime? SlutDato { get; set; }

    // Minimum ordrebeløb for at rabatten gælder
    public decimal? MinimumBeløb { get; set; }

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

    public bool IsEligibleFor(Kunde? kunde)
    {
        // Kampagnerabat → ingen loyalty-krav
        if (IsKampagne)
            return true;

        // Ingen loyalty-krav → alle kan få
        if (RequiredLoyaltyTier is null)
            return true;

        // Stamkunde-rabat kræver en kunde
        if (kunde is null)
            return false;

        // Kræv at kundens tier er mindst den krævede (Bronze/Silver/Gold)
        return kunde.LoyaltyTier >= RequiredLoyaltyTier.Value;
    }

    public decimal Apply(decimal originalPrice)
    {
        if (Percentage is > 0)
        {
            return originalPrice * (1 - Percentage.Value);
        }

        if (FixedAmount is > 0)
        {
            return Math.Max(0, originalPrice - FixedAmount.Value);
        }

        return originalPrice;
    }
}
