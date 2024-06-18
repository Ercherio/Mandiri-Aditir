using System;
using System.Collections.Generic;

namespace MerchantService.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string MobilePhone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? PasswordChange { get; set; }

    public int Status { get; set; }

    public DateOnly? FirstLoginAt { get; set; }

    public DateOnly? LastLoginAt { get; set; }

    public int? AttemptLoginFailed { get; set; }

    public string? Token { get; set; }

    public DateOnly? LastLoginFailed { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AuthAssignment> AuthAssignments { get; set; } = new List<AuthAssignment>();

    public virtual ICollection<OwnerMerchant> OwnerMerchants { get; set; } = new List<OwnerMerchant>();
}
