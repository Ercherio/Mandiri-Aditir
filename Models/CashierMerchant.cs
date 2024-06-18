using System;
using System.Collections.Generic;

namespace MerchantService.Models;

public partial class CashierMerchant
{
    public int UserId { get; set; }

    public string? Mid { get; set; }

    public virtual Merchant? MidNavigation { get; set; }
}
