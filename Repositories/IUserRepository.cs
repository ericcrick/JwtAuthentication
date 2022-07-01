using JwtAuthentication.Models;

namespace JwtAuthentication.Repositories
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<User> FindById(int id);
    }
}