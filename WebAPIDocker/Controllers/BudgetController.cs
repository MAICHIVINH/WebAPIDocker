using Application.DTOs;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Share;
namespace WebAPIDocker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController
{
    private IBudgetService _service;

    public BudgetController(IBudgetService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<Result<int>> CreateBudget([FromQuery] int userId, [FromBody] CreateBudgetDto dto)
    {
        return await _service.CreateBudget(userId, dto);
    }
    
    [HttpPost]
    [Route("update/{id}")]
    public async Task<Result<bool>> UpdateBudget(int id, [FromQuery] int userId, [FromBody] UpdateBudgetDto dto)
    {
        return await _service.UpdateBudget(userId, id, dto);
    }
    
    [HttpPost]
    [Route("delete/{id}")] 
    public async Task<Result<bool>> DeleteBudget(int budgetId, [FromQuery] int userId)
    {
        return await _service.DeleteBudget(userId, budgetId);
    }
    
    [HttpGet("get-budgets")]
    public async Task<Result<List<BudgetDto>>> GetBudgets([FromQuery] int userId, [FromQuery] string? period = null, [FromQuery] int? categoryId = null)
    {
        return await _service.GetBudgets(userId, period, categoryId);
    }

    [HttpGet]
    [Route("get-budget-by-id/{budgetId}")]
    public async Task<Result<BudgetDto?>> GetBudgetById(int budgetId, [FromQuery] int userId)
    {
        return await _service.GetBudgetById(userId, budgetId);
    }
    
    [HttpGet]
    [Route("get-budget-usage/{budgetId}")]
    public async Task<Result<BudgetUsageDto?>> GetBudgetUsage(int budgetId, [FromQuery] int userId)
    {
        return await _service.GetBudgetUsage(userId, budgetId);
    }
    
    [HttpGet]
    [Route("check-exceeded")]
    public async Task<Result<bool>> CheckBudgetExceeded([FromQuery] int userId, [FromQuery] int categoryId, [FromQuery] DateTime date, [FromQuery] decimal transactionAmount)
    {
        return await _service.CheckBudgetExceeded(userId, categoryId, date, transactionAmount);
        
    }
    
}