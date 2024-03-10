using Chatter.Application.Dtos.Chats;
using Chatter.Persistence.RepositoryManagement.Base;
using Chatter.Persistence.RepositoryManagement.EfCore.Users;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ChatMessage = Chatter.Domain.Entities.NoSql.ChatMessage;

namespace Chatter.Application.Services.Chats;

public class ChatService : IChatService
{
    private readonly IBaseRepository<ChatMessage, string> _chatRepository;
    private readonly IUserRepository _userRepository;

    public ChatService(IBaseRepository<ChatMessage, string> chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
    }

    public async Task<Dtos.Chats.ChatMessage> SendMessageAsync(SendChatMessageInput sendChatMessageInput)
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<SendChatMessageInput, ChatMessage>()
            .Map(dest => dest.SentDate, src => DateTime.UtcNow);
        var chatMessage = sendChatMessageInput.Adapt<ChatMessage>(config);

        await _chatRepository.CreateAsync(chatMessage);
        return chatMessage.Adapt<Dtos.Chats.ChatMessage>(); 
    }

    public async Task<List<Dtos.Chats.ChatMessage>> GetChatMessagesAsync(int roomId)
    {
        var chatMessages = _chatRepository.Query()
            .Where(x => x.RoomId == roomId)
            .OrderBy(x => x.SentDate)
            .ProjectToType<Dtos.Chats.ChatMessage>().ToList();

        var users = _userRepository.Query()
            .Where(x => chatMessages.Select(chatMessage => chatMessage.SenderUserId).Contains(x.Id))
            .ToList();

        chatMessages.ForEach(x =>
        {
            x.UserInfo = users.FirstOrDefault(u => u.Id == x.SenderUserId)!.Adapt<Dtos.Users.UserShortInfoDto>();
        });
        return chatMessages;
    }

    public async Task<Dtos.Chats.ChatMessage> GetLastMessageAsync(int roomId)
    {
        var chatMessage =  _chatRepository.Query()
            .Where(x => x.RoomId == roomId)
            .OrderByDescending(x => x.SentDate)
            .ProjectToType<Dtos.Chats.ChatMessage>().FirstOrDefault();


        if(chatMessage is null)
            return null;
        
        var users = await _userRepository.Query()
            .FirstOrDefaultAsync(x => x.Id == chatMessage.SenderUserId);
        
        chatMessage.UserInfo = users?.Adapt<Dtos.Users.UserShortInfoDto>();
        return chatMessage;
    }
}