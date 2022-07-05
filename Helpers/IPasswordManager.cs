using JwtAuthentication.Models;

namespace JwtAuthentication.Helpers
{
    public interface IPasswordManager
    {
         public string HashPassword(string password);
         Task <User> VerifyUserPassword(string email, string password);
    }
}