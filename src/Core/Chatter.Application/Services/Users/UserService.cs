using Chatter.Application.Dtos.Invitations;
using Chatter.Application.Dtos.Users;
using Chatter.Persistence.RepositoryManagement.EfCore.Invitations;
using Chatter.Persistence.RepositoryManagement.EfCore.Users;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Application.Services.Users;

public class UserService : BaseService, IUserService
{
    private readonly IUserRepository _chatterUserRepository;
    private readonly IInvitationRepository _invitationRepository;
    public UserService(IUserRepository chatterUserRepository, IInvitationRepository ınvitationRepository)
    {
        _chatterUserRepository = chatterUserRepository;
        _invitationRepository = ınvitationRepository;
    }

    public async Task<List<UserShortInfoDto>> GetUsersShortInfoAsync(string searchValue)
    {
        return await _chatterUserRepository.Query().Where(x => x.UserName.ToLower().Contains(searchValue.ToLower()))
            .ProjectToType<UserShortInfoDto>().ToListAsync();
    }

    public async Task<List<PendingInvitationDto>> GetPendingInvitationsAsync(string userId)
    {
        return _invitationRepository.Query()
            .Include(x => x.Room)
            .Include(x => x.SenderUser)
            .Where(x => x.InvitedUserId == userId).Adapt<List<PendingInvitationDto>>();
    }
}