using Chatter.Domain.Entities.EFCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Persistence.Identity.Context;

public class AppIdentityDbContext : IdentityDbContext<ChatterUser> 
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {

    }
    
}