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
        bool IsAllowedFor(Kunde? kunde);
        decimal Apply(decimal originalPrice);
        bool IsKampagne { get; }
        string Navn { get; }
        Rabat RabatObjekt { get; }
    }
}
