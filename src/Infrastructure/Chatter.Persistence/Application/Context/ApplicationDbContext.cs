using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Domain.Entities.EFCore.Identity;
using Chatter.Persistence.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Persistence.Application.Context;

public class ApplicationDbContext : IdentityDbContext<ChatterUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ChatterUser>()
            .HasMany(x => x.SentInvitations)
            .WithOne(x => x.SenderUser)
            .HasForeignKey(x => x.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ChatterUser>()
            .HasMany(x => x.ReceivedInvitations)
            .WithOne(x => x.InvitedUser)
            .HasForeignKey(x => x.InvitedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RoomChatterUser>()
            .HasOne(x => x.ChatterUser)
            .WithMany(x => x.RoomChatterUsers)
            .HasForeignKey(x => x.ChatterUserId);

        builder.Entity<RoomChatterUser>()
            .HasOne(x => x.Room)
            .WithMany(x => x.RoomChatterUsers)
            .HasForeignKey(x => x.RoomId);

        base.OnModelCreating(builder);
    }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<RoomPermission> RoomPermissions => Set<RoomPermission>();
}