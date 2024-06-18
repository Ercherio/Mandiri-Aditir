using System;
using System.Collections.Generic;

namespace MerchantService.Models;

public partial class AuthItem
{
    public string Name { get; set; } = null!;

    public int? Type { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<AuthAssignment> AuthAssignments { get; set; } = new List<AuthAssignment>();

    public virtual ICollection<AuthItemChild> AuthItemChildChildNavigations { get; set; } = new List<AuthItemChild>();

    public virtual ICollection<AuthItemChild> AuthItemChildParentNavigations { get; set; } = new List<AuthItemChild>();
}
