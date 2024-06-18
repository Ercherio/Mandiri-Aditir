using System;
using System.Collections.Generic;

namespace MerchantService.Models;

public partial class Merchant
{
    public string Mid { get; set; } = null!;

    public string? Nama { get; set; }

    public virtual ICollection<CashierMerchant> CashierMerchants { get; set; } = new List<CashierMerchant>();
}
