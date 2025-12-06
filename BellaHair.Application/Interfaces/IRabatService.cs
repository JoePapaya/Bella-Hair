    using BellaHair.Domain.Entities;

namespace BellaHair.Application.Interfaces;

public interface IRabatService
{
    DiscountResult BeregnBedsteRabat(
        decimal originalPrice,
        Kunde? kunde,
        string? valgtRabatCode,
        DateTime? bookingDate = null);

    IEnumerable<Rabat> GetTilgængeligeRabatterForKunde(Kunde? kunde);
}
