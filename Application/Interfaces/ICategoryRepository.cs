
using Domain.Entities;

namespace Application.Infrastructure.Repositories
{

    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll(int userId);
        Task<Category> GetById(int id, int userId);
        Task<Category> Add(Category category);
        Task Update(Category category);
        Task Delete(Category category);
    }
}