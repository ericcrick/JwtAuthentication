namespace JwtAuthentication.Dtos
{
    public class LogInUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}