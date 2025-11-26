// BellaHair.Application/Interfaces/IStatisticsApplicationService.cs
using System;

namespace BellaHair.Application.Interfaces;

using BellaHair.Application.Statistics;

public interface IStatisticsApplicationService
{
    DiscountStatistics GetDiscountStatistics(DateTime? from = null, DateTime? to = null);
}
