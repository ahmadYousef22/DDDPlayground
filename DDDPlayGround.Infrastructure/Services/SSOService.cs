using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DDDPlayGround.Infrastructure.Services
{
    public class SSOService
    {
        private readonly IConfiguration _configuration;

        public SSOService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string username, string email, List<string>? roles = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Use JwtSettings configuration (shared with API projects)
            var secret = _configuration["JwtSettings:Secret"] ?? _configuration["Jwt:Key"] 
                ?? throw new InvalidOperationException("JWT Secret key not configured");
            var issuer = _configuration["JwtSettings:Issuer"] ?? _configuration["Jwt:Issuer"] 
                ?? throw new InvalidOperationException("JWT Issuer not configured");
            var audience = _configuration["JwtSettings:Audience"] ?? _configuration["Jwt:Audience"] 
                ?? throw new InvalidOperationException("JWT Audience not configured");
            var expiryMinutes = int.TryParse(_configuration["JwtSettings:ExpiryMinutes"], out var minutes) 
                ? minutes : 60;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                
                // Use JwtSettings configuration (shared with API projects)
                var secret = _configuration["JwtSettings:Secret"] ?? _configuration["Jwt:Key"] 
                    ?? throw new InvalidOperationException("JWT Secret key not configured");
                var issuer = _configuration["JwtSettings:Issuer"] ?? _configuration["Jwt:Issuer"] 
                    ?? throw new InvalidOperationException("JWT Issuer not configured");
                var audience = _configuration["JwtSettings:Audience"] ?? _configuration["Jwt:Audience"] 
                    ?? throw new InvalidOperationException("JWT Audience not configured");
                
                var key = Encoding.UTF8.GetBytes(secret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string GetOAuthAuthorizationUrl(string provider, string redirectUri, string? state = null)
        {
            var clientId = _configuration[$"OAuth:{provider}:ClientId"] ?? "";
            var authUrl = provider.ToLower() switch
            {
                "google" => $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type=code&scope=openid%20email%20profile&state={state ?? Guid.NewGuid().ToString()}",
                _ => throw new ArgumentException($"Unsupported provider: {provider}")
            };

            return authUrl;
        }

        public Dictionary<string, string> GenerateSamlAssertion(string username, string email)
        {
            return new Dictionary<string, string>
            {
                ["AssertionId"] = Guid.NewGuid().ToString(),
                ["Issuer"] = "https://idp.example.com",
                ["Subject"] = username,
                ["Email"] = email,
                ["IssueInstant"] = DateTime.UtcNow.ToString("o"),
                ["NotOnOrAfter"] = DateTime.UtcNow.AddHours(1).ToString("o"),
                ["Audience"] = "https://sp.example.com"
            };
        }

        public List<string> GetAvailableSSOOptions()
        {
            return new List<string>
            {
                "JWT",
                "OAuth2_Google",
                "SAML2"
            };
        }
    }
}
