namespace Application.DTOs
{

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Role { get; set; } = "USER";
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class ValidateTokenResponse
    {
        public bool IsValid { get; set; }
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? Message { get; set; }
    }
}