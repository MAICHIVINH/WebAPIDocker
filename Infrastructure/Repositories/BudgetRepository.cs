using Application.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly ExpenseDbContext _db ;

    public BudgetRepository(ExpenseDbContext db)
    {
        _db = db;
    }

    public async Task<Budget?> GetById(int id, int userId)
    {
        return await _db.Budgets.FirstOrDefaultAsync(x => x.id == id && x.user_id == userId);
    }

    public async Task<IEnumerable<Budget>> GetByUser(int userId)
    {
        return await _db.Budgets.Where(x => x.user_id == userId).ToListAsync();
    }

    public async Task<Budget> Add(Budget budget)
    {
        _db.Budgets.Add(budget);
        await _db.SaveChangesAsync();
        return budget;
    }

    public async Task Update(Budget budget)
    {
        try
        {
            _db.Budgets.Update(budget);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var innerMessage = ex.InnerException?.Message;
            throw new Exception($"Lỗi khi update Budget: {innerMessage}", ex);
        }

    }

    public async Task Delete(Budget budget)
    {
        _db.Budgets.Remove(budget);
        await _db.SaveChangesAsync();
    }
}