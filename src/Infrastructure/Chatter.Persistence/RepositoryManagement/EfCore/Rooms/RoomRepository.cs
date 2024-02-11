using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Persistence.Application.Context;
using Chatter.Persistence.RepositoryManagement.EfCore.Base;

namespace Chatter.Persistence.RepositoryManagement.EfCore.Rooms;

public class RoomRepository : EfCoreBaseRepository<Room,int>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
    }
}