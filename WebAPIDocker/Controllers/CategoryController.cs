using Application.DTOs;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share;

namespace WebAPIDocker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }
    
    private int CurrentUserId => 1;

    [HttpGet("get-all")]
    public async Task<Result<IEnumerable<CategoryDto>>> GetAll([FromQuery] int userId)
    {
        return await _service.GetAll(userId);
    }

    [HttpGet]
    [Route("get-by-id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetById(id, CurrentUserId);
        if (category == null) return NotFound(Result<string>.Failure("Loại này không tồn tại"));
        return Ok(category);
    }

    [HttpPost("create")]
    public async Task<Result<int>> Create([FromQuery] int? userId,[FromBody] CreateCategoryDto dto)
    {
        if (userId == null)
        {
            return Result<int>.Failure("Cần đăng nhập để tạo mới");
        }
        return await _service.Create(dto, userId.Value);
    }

    [HttpPost("update/{id}")]
    public async Task<Result<int>> Update(int id , [FromBody] UpdateCategoryDto dto, [FromQuery] int userId)
    {
        if (userId == null)
        {
            return Result<int>.Failure("Cần đăng nhập để tạo mới");
        }
        return await _service.Update(id, dto, CurrentUserId);
    }

    [HttpPost("delete/{id}")]
    public async Task<Result<int>> Delete(int id, [FromQuery] int userId)
    {
        if (userId == null)
        {
            return Result<int>.Failure("Cần đăng nhập để tạo mới");
        }
        return await _service.Delete(id, CurrentUserId);
    }
}