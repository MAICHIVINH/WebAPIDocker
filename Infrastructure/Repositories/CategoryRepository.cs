using Application.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ExpenseDbContext _db ;

    public CategoryRepository(ExpenseDbContext context)
    {
        _db = context;
    }

    public async Task<IEnumerable<Category>> GetAll(int userId)
    {
        return await _db.Categories.Where(c => c.UserId == userId || c.UserId == null ).ToListAsync();
    }

    public async Task<Category?> GetById(int id, int userId)
    {
        return await _db.Categories.Where(c => c.UserId == userId  
                                               && c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Category> Add(Category category)
    {
        try
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return category;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Save error: " + ex.InnerException?.Message);
            throw;
        }
    }

    public async Task Update(Category category)
    {
        try {
        await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Save error: " + ex.InnerException?.Message);
            throw;
        }
    }

    public async Task Delete(Category category)
    {
        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
    }
}