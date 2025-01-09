using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.DataSeedings;
using PoemTown.Repository.Entities;

namespace PoemTown.Repository.Base;

public class PoemTownDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public PoemTownDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            string tableName = entityType.GetTableName() ?? "";
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
        
        builder.Entity<User>().HasData(AccountDataSeeding.DefaultUsers);
        builder.Entity<Role>().HasData(RoleDataSeeding.DefaultRoles);
        builder.Entity<UserRole>().HasData(UserRoleDataSeeding.DefaultUserRoles);

        builder.Entity<UserToken>()
            .HasKey(ut => ut.Id);
        
        builder.Entity<UserToken>()
            .HasIndex(ut => ut.UserId)
            .IsUnique(false);  // Index to speed up queries, but no uniqueness
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}