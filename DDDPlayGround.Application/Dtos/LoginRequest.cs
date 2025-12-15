namespace DDDPlayGround.Application.Dtos
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string>? Roles { get; set; }
    }
}
