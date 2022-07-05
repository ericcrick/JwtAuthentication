using JwtAuthentication.Dtos;
using JwtAuthentication.Models;
using JwtAuthentication.Repositories;

namespace JwtAuthentication.Helpers
{
    public class PasswordManager
    {
        private readonly IUserRepository _repository;
        public PasswordManager(IUserRepository reposiory)
        {
            _repository = reposiory;
        }

        public string HashPassword(string password){
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<User> VerifyUserPassword(LogInUserDto userDto){
            var user = await _repository.GetByEmail(userDto.Email);
            if(user == null) return null!;
            var checkPassword = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);
            if(!checkPassword) return null!;
            return user;
        }
    }
}