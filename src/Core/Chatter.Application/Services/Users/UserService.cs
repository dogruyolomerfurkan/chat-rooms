using Chatter.Application.Dtos.Users;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Persistence.RepositoryManagement.Base;
using Chatter.Persistence.RepositoryManagement.EfCore.Users;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Chatter.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _chatterUserRepository;

    public UserService(IUserRepository chatterUserRepository)
    {
        _chatterUserRepository = chatterUserRepository;
    }

    public async Task<List<SearchUserShortInfoDto>> GetUsersShortInfoAsync(string searchValue)
    {
        return await _chatterUserRepository.Query().Where(x => x.UserName.ToLower().Contains(searchValue.ToLower()))
            .ProjectToType<SearchUserShortInfoDto>().ToListAsync();
    }
}