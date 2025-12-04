using System;
using System.Collections.Generic;
using System.Linq;
using BellaHair.Application.Interfaces;
using BellaHair.Application.Statistics;
using BellaHair.Domain.Entities;

namespace BellaHair.Application.Services;

public class StatisticsApplicationService : IStatisticsApplicationService
{
    private readonly IDataService _dataService;
    private readonly IRabatService _rabatService;

    public StatisticsApplicationService(IDataService dataService, IRabatService rabatService)
    {
        _dataService = dataService;
        _rabatService = rabatService;
    }

    public DiscountStatistics GetDiscountStatistics(DateTime? from = null, DateTime? to = null)
    {
        var bookingsQuery = _dataService.Bookinger.AsQueryable();

        // Filtrér på dato (Booking.Tidspunkt)
        if (from.HasValue)
            bookingsQuery = bookingsQuery.Where(b => b.Tidspunkt >= from.Value);

        if (to.HasValue)
            bookingsQuery = bookingsQuery.Where(b => b.Tidspunkt < to.Value);

        var bookings = bookingsQuery.ToList();

        if (!bookings.Any())
            return new DiscountStatistics();

        int totalBookings = bookings.Count;

        decimal totalRevenueBefore = 0m;
        decimal totalDiscountAmount = 0m;

        var stamkundeRows = new List<BookingDiscountRow>();
        var kampagneRows = new List<BookingDiscountRow>();

        foreach (var booking in bookings)
        {
            // Find behandling og kunde (brug navigation hvis den er loaded, ellers slå op)
            var behandling = booking.Behandling
                              ?? _dataService.Behandlinger.FirstOrDefault(b => b.BehandlingId == booking.BehandlingId);

            var kunde = booking.Kunde
                        ?? _dataService.Kunder.FirstOrDefault(k => k.KundeId == booking.KundeId);

            if (behandling == null)
                continue; // uden pris kan vi ikke regne rabat

            // 🔹 Brug samme logik som BookingPage
            var calc = _rabatService.BeregnBedsteRabat(
                behandling.Pris,
                kunde,
                null,
                booking.Tidspunkt// altid automatisk bedste rabat
            );

            var basePrice = calc.OriginalPrice;
            var finalPrice = calc.FinalPrice;
            var discount = basePrice - finalPrice;
            if (discount < 0)
                discount = 0;

            totalRevenueBefore += basePrice;
            totalDiscountAmount += discount;

            var rabat = calc.AppliedDiscount;
            if (rabat == null)
                continue; // ingen rabat på denne booking

            if (IsStamkundeRabat(rabat))
            {
                stamkundeRows.Add(new BookingDiscountRow(basePrice, discount));
            }
            else if (IsKampagneRabat(rabat))
            {
                kampagneRows.Add(new BookingDiscountRow(basePrice, discount));
            }
        }

        DiscountBreakdownDto BuildBreakdown(string name, List<BookingDiscountRow> rows)
        {
            var revenueBefore = rows.Sum(x => x.BasePrice);
            var discount = rows.Sum(x => x.Discount);

            return new DiscountBreakdownDto
            {
                Name = name,
                NumberOfBookings = rows.Count,
                RevenueBeforeDiscount = revenueBefore,
                DiscountAmount = discount,
                ShareOfAllBookingsPercent =
                    totalBookings == 0 ? 0 : (decimal)rows.Count / totalBookings * 100m
            };
        }

        return new DiscountStatistics
        {
            TotalRevenueBeforeDiscount = totalRevenueBefore,
            TotalDiscountAmount = totalDiscountAmount,
            Stamkunde = BuildBreakdown("Stamkunderabat", stamkundeRows),
            Kampagner = BuildBreakdown("Kampagner", kampagneRows)
        };
    }
    // Lille record til at holde pr. booking-data
    private sealed record BookingDiscountRow(decimal BasePrice, decimal Discount);

    // Stamkunderabat: alt der IKKE er kampagne
    private static bool IsStamkundeRabat(Rabat r) =>
        !r.IsKampagne;

    // Kampagner: baseret på IsKampagne-flag
    private static bool IsKampagneRabat(Rabat r) =>
        r.IsKampagne;
}
