
using Domain.Entities;

namespace Application.Infrastructure.Repositories
{

    public interface IMessageRepository
    {
        Task<Message> AddAsync(Message message);
        Task<IEnumerable<Message>> GetConversationAsync(int userId1, int userId2);
        Task<Message?> GetByIdAsync(int id);
        Task UpdateAsync(Message message);
    }
}