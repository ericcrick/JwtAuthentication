using System.IdentityModel.Tokens.Jwt;

namespace JwtAuthentication.Helpers
{
    public interface IJwtService
    {
        string GenerateToken(int id);
        JwtSecurityToken VerifyToken(string token);
    }
}