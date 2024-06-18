using System;
using System.Collections.Generic;

namespace MerchantService.Models;

public partial class OwnerMerchant
{
    public string Mid { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
