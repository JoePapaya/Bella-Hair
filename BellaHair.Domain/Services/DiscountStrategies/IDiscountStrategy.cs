using BellaHair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Services.DiscountStrategies
{
    public interface IDiscountStrategy
    {
        Rabat RabatObjekt { get; }
        bool IsKampagne { get; }
        bool IsAllowedFor(Kunde? kunde, decimal originalPrice, DateTime dato);
        decimal Apply(decimal originalPrice);
        string Navn { get; }
    }
}
