using Application.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ExpenseDbContext _db;

    public MessageRepository(ExpenseDbContext db)
    {
        _db = db;
    }

    public async Task<Message> AddAsync(Message message)
    {
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();
        return message;
    }

    public async Task<IEnumerable<Message>> GetConversationAsync(int userId1, int userId2)
    {
        return await _db.Messages
            .Where(m => (m.FromUserId == userId1 && m.ToUserId == userId2) ||
                        (m.FromUserId == userId2 && m.ToUserId == userId1))
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        return await _db.Messages.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task UpdateAsync(Message message)
    {
        _db.Messages.Update(message);
        await _db.SaveChangesAsync();
    }
}