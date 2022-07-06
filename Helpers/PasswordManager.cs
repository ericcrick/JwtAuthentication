using JwtAuthentication.Models;
using JwtAuthentication.Repositories;

namespace JwtAuthentication.Helpers
{
    public class PasswordManager: IPasswordManager
    {
        public string HashPassword(string password){
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyUserPassword(string loginPassword, string dbPassword){
            return BCrypt.Net.BCrypt.Verify(loginPassword, dbPassword);
        }
    }
}