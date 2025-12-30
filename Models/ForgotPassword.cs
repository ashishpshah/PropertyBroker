using System;
using System.Collections.Generic;

namespace Broker.Models;

public class ForgotPassword
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string OTP { get; set; }

    // Add these properties to match your table
    public DateTime CreatedAt { get; set; }
    public bool IsUsed { get; set; }
}
