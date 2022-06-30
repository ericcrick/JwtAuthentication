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
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<User>(entity => entity.HasIndex(e => e.Email).IsUnique());
        }
    }
}