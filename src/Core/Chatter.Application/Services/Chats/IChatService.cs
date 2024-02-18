using Chatter.Application.Dtos.Chats;

namespace Chatter.Application.Services.Chats;

public interface IChatService
{
    Task<ChatMessage> SendMessageAsync(SendChatMessageInput sendChatMessageInput);
    
    Task<List<ChatMessage>> GetChatMessagesAsync(int roomId);
}

