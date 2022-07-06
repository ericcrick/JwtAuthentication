using JwtAuthentication.Models;

namespace JwtAuthentication.Helpers
{
    public interface IPasswordManager
    {
         public string HashPassword(string password);
         public bool VerifyUserPassword(string loginPassword, string dbPassword);
    }
}