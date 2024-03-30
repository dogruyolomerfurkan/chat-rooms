using Chatter.Application.Dtos.Users;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Persistence.RepositoryManagement.EfCore.Invitations;
using Chatter.Persistence.RepositoryManagement.EfCore.Users;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Application.Services.Users;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _chatterUserRepository;
    private readonly IInvitationRepository _invitationRepository;

    public UserService(IUserRepository chatterUserRepository, IInvitationRepository invitationRepository,
        UserManager<ChatterUser> userManager) : base(userManager)
    {
        _chatterUserRepository = chatterUserRepository;
        _invitationRepository = invitationRepository;
    }

    public async Task<List<UserShortInfoDto>> GetUsersShortInfoAsync(string searchValue)
    {
        return await _chatterUserRepository.Query().Where(x => x.UserName.ToLower().Contains(searchValue.ToLower()))
            .ProjectToType<UserShortInfoDto>().ToListAsync();
    }
}