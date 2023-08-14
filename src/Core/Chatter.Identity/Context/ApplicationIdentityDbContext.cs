using Chatter.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Identity.Context;

public class ApplicationIdentityDbContext : IdentityDbContext<ChatterUser> 
{

    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
    {

    }
}