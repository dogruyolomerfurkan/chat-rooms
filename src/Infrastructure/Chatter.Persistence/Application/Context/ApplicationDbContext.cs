using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Domain.Entities.EFCore.Identity;
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

        builder.Entity<Room>()
            .HasMany(x => x.Users)
            .WithMany(x => x.ChatRooms)
            .UsingEntity("JTable_RoomUsers");
        
        builder.Entity<Room>()
            .HasMany(x => x.BlockedUsers)
            .WithMany(x => x.BlockedRooms)
            .UsingEntity("JTable_RoomBlockedUsers");
        
        base.OnModelCreating(builder);
    }   
        
        
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Invitation> Invitations  => Set<Invitation>();
}