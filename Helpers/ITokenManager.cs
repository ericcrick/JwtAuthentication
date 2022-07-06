using System.Security.Claims;
using JwtAuthentication.Models;

namespace JwtAuthentication.Helpers
{
    public interface ITokenManager
    {
         public string GenerateToken(User user);
         public ClaimsPrincipal ValidateToken(string token);
    }
}