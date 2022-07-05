using System.Security.Claims;
using JwtAuthentication.Repositories;

namespace JwtAuthentication.Helpers
{
    public class TokenManager: ITokenManager
    {
        private readonly IUserRepository _userRepository;
        public TokenManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string GenerateToken(string email, string password)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}