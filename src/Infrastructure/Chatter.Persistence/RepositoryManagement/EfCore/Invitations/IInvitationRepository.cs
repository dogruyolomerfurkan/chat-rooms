using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Persistence.RepositoryManagement.Base;

namespace Chatter.Persistence.RepositoryManagement.EfCore.Invitations;

public interface IInvitationRepository : IBaseRepository<Invitation,int>
{
    
}