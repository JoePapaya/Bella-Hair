using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// BellaHair.Application/Statistics/DiscountStatisticsDto.cs
namespace BellaHair.Application.Statistics;

public class DiscountStatistics
{
    public DiscountBreakdownDto Stamkunde { get; set; } = new();
    public DiscountBreakdownDto Kampagner { get; set; } = new();

    public decimal TotalRevenueBeforeDiscount { get; set; }
    public decimal TotalDiscountAmount { get; set; }

    public decimal TotalRevenueAfterDiscount => TotalRevenueBeforeDiscount - TotalDiscountAmount;
}

public class DiscountBreakdownDto
{
    public string Name { get; set; } = string.Empty;

    public int NumberOfBookings { get; set; }

    /// <summary>Sum of discount amount for this type</summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>Revenue before discount for this type</summary>
    public decimal RevenueBeforeDiscount { get; set; }

    public decimal RevenueAfterDiscount => RevenueBeforeDiscount - DiscountAmount;

    /// <summary>Average discount per booking that got this type</summary>
    public decimal AverageDiscountPerBooking =>
        NumberOfBookings == 0 ? 0 : DiscountAmount / NumberOfBookings;

    /// <summary>Share of all bookings (0–100%)</summary>
    public decimal ShareOfAllBookingsPercent { get; set; }
}
