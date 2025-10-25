
using Application.DTOs;
using Share;

namespace Application.Infrastructure.Repositories
{

    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDto>>> GetAll(int userId);
        Task<CategoryDto?> GetById(int id, int userId);
        Task<Result<int>> Create(CreateCategoryDto dto, int userId);
        Task<Result<int>> Update(int id, UpdateCategoryDto dto, int userId);
        Task<Result<int>> Delete(int id, int userId);
    }
}