using JwtAuthentication.Data;
using JwtAuthentication.Models;

namespace JwtAuthentication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _usercontext;
        public UserRepository(UserDbContext userContext)
        {
            _usercontext = userContext;
        }

        public async Task<User> Create(User user)
        {
           await _usercontext.Users.AddAsync(user);
          user.Id =  await _usercontext.SaveChangesAsync();
           return user;
        }
    }
}