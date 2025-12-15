using DDDPlayGround.Application.Dtos;
using DDDPlayGround.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDPlayGround.SSO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SSOController : ControllerBase
    {
        private readonly SSOService _ssoService;
        private readonly ILogger<SSOController> _logger;
        public SSOController(SSOService ssoService, ILogger<SSOController> logger)
        {
            _ssoService = ssoService;
            _logger = logger;
        }

        [HttpGet("options")]
        [Authorize]
        public IActionResult GetSSOOptions()
        {
            var options = _ssoService.GetAvailableSSOOptions();
            return Ok(new
            {
                AvailableOptions = options,
                Description = "Available SSO authentication methods"
            });
        }

        [HttpPost("jwt/login")]
        public IActionResult LoginWithJWT([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { Error = "Username and Email are required" });
            }

            var token = _ssoService.GenerateJwtToken(
                request.Username,
                request.Email,
                request.Roles ?? new List<string> { "User" });

            return Ok(new
            {
                Token = token,
                TokenType = "Bearer",
                ExpiresIn = 3600,
                Message = "JWT token generated successfully"
            });
        }

        [HttpPost("jwt/validate")]
        public IActionResult ValidateJWT([FromBody] ValidateTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new { Error = "Token is required" });
            }

            var principal = _ssoService.ValidateJwtToken(request.Token);

            if (principal == null)
            {
                return Unauthorized(new { Error = "Invalid or expired token" });
            }

            var claims = principal.Claims.Select(c => new
            {
                c.Type,
                c.Value
            }).ToList();

            return Ok(new
            {
                Valid = true,
                Claims = claims,
                Message = "Token is valid"
            });
        }

        [HttpGet("oauth/authorize")]
        public IActionResult GetOAuthUrl([FromQuery] string provider, [FromQuery] string redirectUri, [FromQuery] string? state = null)
        {
            if (string.IsNullOrEmpty(provider) || string.IsNullOrEmpty(redirectUri))
            {
                return BadRequest(new { Error = "Provider and RedirectUri are required" });
            }

            try
            {
                var authUrl = _ssoService.GetOAuthAuthorizationUrl(provider, redirectUri, state);
                return Ok(new
                {
                    Provider = provider,
                    AuthorizationUrl = authUrl,
                    RedirectUri = redirectUri,
                    State = state,
                    Message = $"OAuth 2.0 authorization URL for {provider}"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("oauth/callback")]
        [HttpGet("/callback")] 
        public IActionResult OAuthCallback([FromQuery] string? code, [FromQuery] string? state, [FromQuery] string? error)
        {
            _logger.LogInformation("OAuth callback received. Code: {Code}, State: {State}, Error: {Error}", code, state, error);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new
                {
                    Error = error,
                    Message = "OAuth authorization was denied or failed"
                });
            }

            if (string.IsNullOrEmpty(code))
            {
                return BadRequest(new
                {
                    Error = "missing_code",
                    Message = "Authorization code is missing"
                });
            }

            return Ok(new
            {
                Success = true,
                AuthorizationCode = code,
                State = state,
                Message = "OAuth callback received successfully!"
            });
        }

        [HttpPost("saml/assertion")]
        public IActionResult GenerateSamlAssertion([FromBody] SamlRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { Error = "Username and Email are required" });
            }

            var assertion = _ssoService.GenerateSamlAssertion(request.Username, request.Email);

            return Ok(new
            {
                Assertion = assertion,
                Message = "SAML assertion generated (simulated for learning purposes)"
            });
        }
    }
}
