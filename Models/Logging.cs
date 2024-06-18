using System;
using System.Collections.Generic;

namespace MerchantService.Models;

public partial class Logging
{
    public int Id { get; set; }

    public string? Action { get; set; }

    public DateTime? Datetime { get; set; }

    public string? IpAddress { get; set; }

    public string? Url { get; set; }

    public string? RequestHeader { get; set; }

    public string? ResponseHeader { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public string? Error { get; set; }

    public string? ErrorDescription { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? FinishedTime { get; set; }

    public int? ElapsedTime { get; set; }
}
