using DDDPlayGround.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DDDPlayGround.Api2.Controllers
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

        /// <summary>
        /// Validates a JWT token issued by the SSO service
        /// </summary>
        [HttpPost("validate")]
        [AllowAnonymous]
        public IActionResult ValidateToken([FromBody] ValidateTokenRequest request)
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
                Message = "Token is valid and issued by SSO service"
            });
        }

        /// <summary>
        /// Get current user information from SSO token
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                Username = username,
                Email = email,
                Roles = roles,
                Message = "User authenticated via SSO token"
            });
        }
    }

    public class ValidateTokenRequest
    {
        public string Token { get; set; } = string.Empty;
    }
}

