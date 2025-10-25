
using Application.DTOs;
using Share;

namespace Application.Infrastructure.Repositories
{

    public interface IUserService
    {
        Task<Result<IEnumerable<UserDto>>> GetAllUsers();

        Task<Result<string>> RegisterAsync(string username, string email, string password);

        Task<Result<string>> LoginAsync(string username, string password);
        Task<Result<ValidateTokenResponse>> ValidateTokenAsync(string token);

    }

}