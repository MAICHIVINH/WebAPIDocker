
using Application.DTOs;
using Share;

namespace Application.Infrastructure.Repositories
{

    public interface IBudgetService
    {
        Task<Result<int>> CreateBudget(int userId, CreateBudgetDto dto);
        Task<Result<bool>> UpdateBudget(int userId, int budgetId, UpdateBudgetDto dto);
        Task<Result<bool>> DeleteBudget(int userId, int budgetId);
        Task<Result<List<BudgetDto>>> GetBudgets(int userId, string? period = null, int? categoryId = null);
        Task<Result<BudgetDto?>> GetBudgetById(int userId, int budgetId);
        Task<Result<BudgetUsageDto?>> GetBudgetUsage(int userId, int budgetId);
        Task<Result<bool>> CheckBudgetExceeded(int userId, int categoryId, DateTime date, decimal transactionAmount);
    }
}