using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Persistence.Application.Context;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Persistence.RepositoryManagement.EfCore.Users;

public class UserRepository : IUserRepository
{
    private readonly DbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
        _context.ChangeTracker.LazyLoadingEnabled = true;
    }
    public IQueryable<ChatterUser> Query(bool asNoTracking = false)
    {
        return asNoTracking
            ? _context.Set<ChatterUser>().AsNoTracking().AsQueryable()
            : _context.Set<ChatterUser>().AsQueryable();
    }
}