using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Entities;

public class DiscountResult
{
    public decimal OriginalPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public Rabat? AppliedDiscount { get; set; }
    public string? AppliedDiscountName { get; set; }

}