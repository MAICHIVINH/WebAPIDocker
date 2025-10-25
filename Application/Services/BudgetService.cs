
using Application.DTOs;
using Application.Infrastructure.Repositories;
using Domain.Entities;
using Share;

namespace Application.Services
{

    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _repository;

        public BudgetService(IBudgetRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<int>> CreateBudget(int userId, CreateBudgetDto dto)
        {
            var budget = new Budget
            {
                user_id = userId,
                category_id = dto.CategoryId,
                amount = dto.Amount,
                period = dto.Period,
                start_date = dto.StartDate,
                end_date = dto.EndDate
            };

            var created = await _repository.Add(budget);
            return await Result<int>.SuccessAsync(created.id, "Tạo ngân sách thành công");
        }

        public async Task<Result<bool>> UpdateBudget(int userId, int budgetId, UpdateBudgetDto dto)
        {
            var budget = await _repository.GetById(budgetId, userId);
            if (budget == null)
                throw new Exception("Ngân sách này không tồn tại");

            budget.amount = dto.Amount;
            budget.period = dto.Period;
            budget.start_date = dto.StartDate;
            budget.end_date = dto.EndDate;




            budget.category_id = dto.CategoryId;

            await _repository.Update(budget);
            return await Result<bool>.SuccessAsync(true, "Cập nhật ngân sách thành công");

        }

        public async Task<Result<bool>> DeleteBudget(int userId, int budgetId)
        {
            var budget = await _repository.GetById(budgetId, userId);
            if (budget == null)
                throw new Exception("Ngân sách này không tồn tại");

            await _repository.Delete(budget);
            return await Result<bool>.SuccessAsync(true, "Xóa ngân sách thành công");
        }

        public async Task<Result<List<BudgetDto>>> GetBudgets(int userId, string? period = null, int? categoryId = null)
        {
            var budgets = await _repository.GetByUser(userId);

            if (!string.IsNullOrEmpty(period))
                budgets = budgets.Where(x => x.period == period);

            if (categoryId.HasValue)
                budgets = budgets.Where(x => x.category_id == categoryId.Value);

            var budgetsGet = budgets.Select(b => new BudgetDto
            {
                Id = b.id,
                CategoryId = b.category_id,
                Amount = b.amount,
                Period = b.period,
                StartDate = b.start_date,
                EndDate = b.end_date
            }).ToList();

            return await Result<List<BudgetDto>>.SuccessAsync(budgetsGet, "Lấy danh sách ngân sách thành công");
        }


        public async Task<Result<BudgetDto?>> GetBudgetById(int userId, int budgetId)
        {
            var b = await _repository.GetById(budgetId, userId);
            if (b == null) return null;

            var budget = new BudgetDto
            {
                Id = b.id,
                CategoryId = b.category_id,
                Amount = b.amount,
                Period = b.period,
                StartDate = b.start_date,
                EndDate = b.end_date
            };

            return await Result<BudgetDto?>.SuccessAsync(budget, "Lấy dữ liệu thành công");
        }

        public async Task<Result<BudgetUsageDto?>> GetBudgetUsage(int userId, int budgetId)
        {
            var b = await _repository.GetById(budgetId, userId);
            if (b == null) return null;

            var spent = 5000;
            var budget = new BudgetUsageDto
            {
                BudgetId = b.id,
                CategoryId = b.category_id,
                LimitAmount = b.amount,
                SpentAmount = spent,
                Remaining = b.amount - spent,
                Percent = b.amount > 0 ? Math.Round((double)(spent / b.amount * 100), 2) : 0
            };

            return await Result<BudgetUsageDto?>.SuccessAsync(budget, "Lấy dữ liệu thành công");
        }

        public async Task<Result<bool>> CheckBudgetExceeded(int userId, int categoryId, DateTime date, decimal transactionAmount)
        {
            var budgets = await _repository.GetByUser(userId);

            var activeBudget = budgets.FirstOrDefault(b =>
                b.category_id == categoryId &&
                b.start_date <= date &&
                b.end_date >= date
            );

            if (activeBudget == null) throw new Exception("Không tồn tại ngân sách nào cả!");

            var spent = 5000;

            var check = spent + transactionAmount > activeBudget.amount;

            return await Result<bool>.SuccessAsync(check, "Đã kiểm tra thành công!");
        }
    }

}