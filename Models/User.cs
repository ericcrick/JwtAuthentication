namespace JwtAuthentication.Models
{
    public class User
    {
        public string Username { set; get; } = string.Empty;
        public byte[] Password { set; get; } = null!;
    }
}