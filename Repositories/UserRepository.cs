using JwtAuthentication.Data;
using JwtAuthentication.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<User> FindById(int id){
            var user = await _usercontext.Users.FirstOrDefaultAsync( user => user.Id == id);
            return user!;
        }
        public async Task<User>GetByEmail(string email){
            var user = await _usercontext.Users.FirstOrDefaultAsync(user => user.Email == email);
            return user!;
        }
    }
}