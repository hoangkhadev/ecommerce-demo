using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Infrastructure.Configurations
{
    public class JwtOptions
    {
        public const string SectionName = "JwtSettings";
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; }

    }
}
