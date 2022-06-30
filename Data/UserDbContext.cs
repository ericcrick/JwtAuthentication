using JwtAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Data
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options): base(options)
        {
        }
        public DbSet<User> Users {get; set;} = null!;
    }
}