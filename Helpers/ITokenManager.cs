using System.Security.Claims;
namespace JwtAuthentication.Helpers
{
    public interface ITokenManager
    {
         public string GenerateToken(string email, string password);
         public ClaimsPrincipal ValidateToken(string token);
    }
}