using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtAuthentication.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.Helpers
{
    public class TokenManager: ITokenManager
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly byte[] secretKey;
        public TokenManager(IUserRepository userRepository)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            secretKey = Encoding.ASCII.GetBytes("MySecurity#8keys");
            _userRepository = userRepository;
        }

        public string GenerateToken(string email, string password)
        {
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Email, email)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(5),
            };
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var claim = tokenHandler.ValidateToken(token, new TokenValidationParameters{
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            return claim;
        }
    }
}