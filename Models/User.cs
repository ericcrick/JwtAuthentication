namespace JwtAuthentication.Models
{
    public class User
    {
        public string Username { set; get; } = string.Empty;
        public byte[] PasswordHash { set; get; } = null!;
        public byte[] PasswordSalt { set; get; } = null!;
    }
}