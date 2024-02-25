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

}