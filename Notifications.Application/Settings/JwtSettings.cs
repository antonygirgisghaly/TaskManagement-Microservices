using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Application.Settings
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpiryMinutes { get; set; }
    }
}
