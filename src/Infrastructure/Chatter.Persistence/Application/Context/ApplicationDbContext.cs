using Chatter.Common.Settings;
using Chatter.Domain.Entities.EFCore.Application;
using Chatter.Domain.Entities.EFCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Chatter.Persistence.Application.Context;

public class ApplicationDbContext : IdentityDbContext<ChatterUser>
{
    private readonly DatabaseSetting _databaseSetting;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IOptions<DatabaseSetting> databaseSetting) : base(options)
    {
        _databaseSetting = databaseSetting.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Room>()
            .HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<ChatterUser>()
            .HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<Invitation>()
            .HasQueryFilter(x => !x.IsDeleted);

        builder.Entity<RoomChatterUser>()
            .HasQueryFilter(x => !x.IsDeleted);

        if (_databaseSetting.Provider == "Postgres")
        {
            builder.Entity<RoomChatterUser>()
                .HasIndex(x => new {x.ChatterUserId, x.RoomId})
                .HasFilter("is_deleted = false");
        }
        else if (_databaseSetting.Provider == "SqlServer")
        {
            builder.Entity<RoomChatterUser>()
                .HasIndex(x => new {x.ChatterUserId, x.RoomId})
                .HasFilter("IsDeleted = 0");
        }

        builder.Entity<RoomChatterUser>()
            .HasOne(x => x.ChatterUser)
            .WithMany(x => x.RoomChatterUsers)
            .HasForeignKey(x => x.ChatterUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RoomChatterUser>()
            .HasOne(x => x.Room)
            .WithMany(x => x.RoomChatterUsers)
            .HasForeignKey(x => x.RoomId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

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


        base.OnModelCreating(builder);
    }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<RoomPermission> RoomPermissions => Set<RoomPermission>();
}