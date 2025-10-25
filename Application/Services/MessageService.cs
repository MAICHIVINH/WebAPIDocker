

using Application.DTOs;
using Application.Infrastructure.Repositories;
using Domain.Entities;
using Share;

namespace sepending.Application.Services
{

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;

        public MessageService(IMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<MessageDto>> SendMessage(int fromUserId, int toUserId, string text)
        {
            var message = new Message
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Text = text,
                Status = "sent",
            };

            var saved = await _repository.AddAsync(message);

            var dto = new MessageDto
            {
                Id = saved.Id,
                FromUserId = saved.FromUserId,
                ToUserId = saved.ToUserId,
                Text = saved.Text,
                Status = saved.Status,
                CreatedAt = saved.CreatedAt
            };

            return await Result<MessageDto>.SuccessAsync(dto, "Gửi tin nhắn thành công");
        }

        public async Task<Result<IEnumerable<MessageDto>>> GetConversation(int userId1, int userId2)
        {
            var messages = await _repository.GetConversationAsync(userId1, userId2);

            var list = messages.Select(m => new MessageDto
            {
                Id = m.Id,
                FromUserId = m.FromUserId,
                ToUserId = m.ToUserId,
                Text = m.Text,
                Status = m.Status,
                CreatedAt = m.CreatedAt
            });

            return await Result<IEnumerable<MessageDto>>.SuccessAsync(list, "Lấy hội thoại thành công");
        }
    }
}