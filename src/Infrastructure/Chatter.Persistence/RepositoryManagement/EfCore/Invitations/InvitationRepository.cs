using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Persistence.Application.Context;
using Chatter.Persistence.RepositoryManagement.EfCore.Base;

namespace Chatter.Persistence.RepositoryManagement.EfCore.Invitations;

public class InvitationRepository : EfCoreBaseRepository<Invitation,int>, IInvitationRepository
{
    public InvitationRepository(ApplicationDbContext context) : base(context)
    {
    }
}