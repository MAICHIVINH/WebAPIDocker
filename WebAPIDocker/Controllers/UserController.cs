using Application.DTOs;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Share;
using LoginRequest = Application.DTOs.LoginRequest;
using RegisterRequest = Application.DTOs.RegisterRequest;

namespace WebAPIDocker.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("get-all")]
    public async Task<Result<IEnumerable<UserDto>>> GetAllUsers()
    {
        return await _userService.GetAllUsers();
    } 
    
    [HttpPost("register")]
    public async Task<ActionResult<Result<string>>> Register([FromBody] RegisterRequest request)
    {
        return await _userService.RegisterAsync(request.Username, request.Email, request.Password);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<Result<string>>> Login([FromBody] LoginRequest request)
    {
        return await _userService.LoginAsync( request.Username, request.Password);
    }

    [HttpGet("validate")]
    public async Task<ActionResult<Result<ValidateTokenResponse>>> ValidateToken([FromQuery] string token)
    {
        return await _userService.ValidateTokenAsync(token);
    }

}