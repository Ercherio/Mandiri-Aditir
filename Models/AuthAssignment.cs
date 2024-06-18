using System;
using System.Collections.Generic;

namespace MerchantService.Models;

public partial class AuthAssignment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ItemName { get; set; } = null!;

    public DateOnly CreatedAt { get; set; }

    public virtual AuthItem ?ItemNameNavigation { get; set; }

    public virtual User? User { get; set; } 
}
