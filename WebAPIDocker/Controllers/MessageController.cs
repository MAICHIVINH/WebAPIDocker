using Application.DTOs;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share;


namespace WebAPIDocker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }
    
    [HttpPost("send")]
    public async Task<Result<MessageDto>> Send([FromBody] SendMessageRequest request)
    {
        return await _messageService.SendMessage(request.FromUserId, request.ToUserId, request.Text);
    }

    [HttpGet("conversation")]
    public async Task<Result<IEnumerable<MessageDto>>> GetConversation([FromQuery] int userId1, [FromQuery] int userId2)
    {
        return await _messageService.GetConversation(userId1, userId2);
    }
}