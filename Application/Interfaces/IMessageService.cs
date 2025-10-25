

using Application.DTOs;
using Share;

namespace Application.Infrastructure.Repositories
{

    public interface IMessageService
    {
        Task<Result<MessageDto>> SendMessage(int fromUserId, int toUserId, string text);
        Task<Result<IEnumerable<MessageDto>>> GetConversation(int userId1, int userId2);
    }
}