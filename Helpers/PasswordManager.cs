using JwtAuthentication.Models;
using JwtAuthentication.Repositories;

namespace JwtAuthentication.Helpers
{
    public class PasswordManager: IPasswordManager
    {
        private readonly IUserRepository _repository;
        public PasswordManager(IUserRepository reposiory)
        {
            _repository = reposiory;
        }

        public string HashPassword(string password){
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<User> VerifyUserPassword(string email, string password){
            var user = await _repository.GetByEmail(email);
            if(user == null) return null!;
            var checkPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if(!checkPassword) return null!;
            return user;
        }
    }
}