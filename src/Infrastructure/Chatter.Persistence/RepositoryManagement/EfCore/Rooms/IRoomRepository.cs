using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Persistence.RepositoryManagement.Base;

namespace Chatter.Persistence.RepositoryManagement.EfCore.Rooms;

public interface IRoomRepository : IBaseRepository<Room,int>
{
    
}