using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DDDPlayGround.Api2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires SSO token
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get protected data - requires SSO authentication
        /// </summary>
        [HttpGet]
        public IActionResult GetData()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                Message = "This is protected data from API 2",
                User = new
                {
                    Username = username,
                    Email = email
                },
                Data = new[]
                {
                    new { Id = 1, Name = "Item 1", Description = "Protected resource 1" },
                    new { Id = 2, Name = "Item 2", Description = "Protected resource 2" },
                    new { Id = 3, Name = "Item 3", Description = "Protected resource 3" }
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Get user-specific data based on SSO token claims
        /// </summary>
        [HttpGet("user-specific")]
        public IActionResult GetUserSpecificData()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                Message = $"User-specific data for {username}",
                User = new
                {
                    Username = username,
                    Email = email,
                    Roles = roles
                },
                UserData = new
                {
                    Preferences = new { Theme = "Dark", Language = "en-US" },
                    LastLogin = DateTime.UtcNow.AddHours(-2),
                    AccessLevel = roles.Contains("Admin") ? "Full" : "Standard"
                }
            });
        }
    }
}

