

using Domain.Entities;

namespace Application.Infrastructure.Repositories
{

    public interface IBudgetRepository
    {
        Task<Budget?> GetById(int id, int userId);
        Task<IEnumerable<Budget>> GetByUser(int userId);
        Task<Budget> Add(Budget budget);
        Task Update(Budget budget);
        Task Delete(Budget budget);
    }
}