using Chatter.Application.Dtos.Rooms;
using Chatter.Application.Dtos.Users;
using Chatter.Persistence.RepositoryManagement.EfCore.Users;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Application.Services.Users;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _chatterUserRepository;
    
    public UserService(IUserRepository chatterUserRepository)
    {
        _chatterUserRepository = chatterUserRepository;
    }

    public async Task<List<UserShortInfoDto>> GetUsersShortInfoAsync(string searchValue)
    {
        return await _chatterUserRepository.Query().Where(x => x.UserName.ToLower().Contains(searchValue.ToLower()))
            .ProjectToType<UserShortInfoDto>().ToListAsync();
    }

    public async Task<List<RoomDto>> GetUserRooms(string userId)
    {
        return await _chatterUserRepository.Query().Where(x => x.Id == userId)
            .Include(x => x.RoomChatterUsers)
            .ThenInclude(x => x.Room).SelectMany(x => x.RoomChatterUsers).Select(x => x.Room)
            .ProjectToType<RoomDto>(CreateTypeAdapterConfig(5)).ToListAsync();
    }
}