
using Domain.Entities;

namespace Application.Infrastructure.Repositories
{

    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();

        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }

}