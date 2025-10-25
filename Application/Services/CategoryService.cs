

using Application.DTOs;
using Application.Infrastructure.Repositories;
using Domain.Entities;
using Share;

namespace Application.Services
{

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task<Result<IEnumerable<CategoryDto>>> GetAll(int userId)
        {
            var list = await _categoryRepository.GetAll(userId);

            var result = list.Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Type,
                userId,
                DateTime.Now
            ));

            return Result<IEnumerable<CategoryDto>>.Success(result, "Lấy dữ liệu thành công");
        }


        public async Task<CategoryDto?> GetById(int id, int userId)
        {
            var category = await _categoryRepository.GetById(id, userId);
            return category == null ? null
                                    : new CategoryDto(category.Id, category.Name, category.Type, userId, DateTime.Now);
        }

        public async Task<Result<int>> Create(CreateCategoryDto dto, int userId)
        {
            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type,
                UserId = userId
            };

            var created = await _categoryRepository.Add(category);

            return await Result<int>.SuccessAsync(created.Id, "Thêm dữ liệu thành công");
        }

        public async Task<Result<int>> Update(int id, UpdateCategoryDto dto, int userId)
        {
            var category = await _categoryRepository.GetById(id, userId);
            if (category == null) throw new Exception("Loại này không tồn tại");

            category.Name = dto.Name;
            category.Type = dto.Type;
            await _categoryRepository.Update(category);

            return await Result<int>.SuccessAsync(category.Id, "Cập nhật dữ liệu thành công");
        }

        public async Task<Result<int>> Delete(int id, int userId)
        {
            var category = await _categoryRepository.GetById(id, userId);
            if (category == null) throw new Exception("Loại này không tồn tại");

            await _categoryRepository.Delete(category);
            return await Result<int>.SuccessAsync("Xóa dữ liệu thành công");
        }
    }
}