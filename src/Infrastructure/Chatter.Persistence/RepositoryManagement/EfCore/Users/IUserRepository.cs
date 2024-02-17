using Chatter.Domain.Entities.EFCore.Identity;

namespace Chatter.Persistence.RepositoryManagement.EfCore.Users;

public interface IUserRepository 
{
    IQueryable<ChatterUser> Query(bool asNoTracking = false);
}