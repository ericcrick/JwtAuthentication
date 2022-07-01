namespace JwtAuthentication.Dtos
{
    public class ReadUserDto
    {
        public int Id { get; set;}
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}