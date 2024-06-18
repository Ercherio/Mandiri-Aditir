using System;
using System.Collections.Generic;

namespace MerchantService.Models;

public partial class AuthItemChild
{
    public int Id { get; set; }

    public string Parent { get; set; } = null!;

    public string Child { get; set; } = null!;

    public virtual AuthItem? ChildNavigation { get; set; }

    public virtual AuthItem? ParentNavigation { get; set; }
}
