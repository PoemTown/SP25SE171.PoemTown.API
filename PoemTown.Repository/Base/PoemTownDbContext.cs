using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PoemTown.Repository.DataSeedings;
using PoemTown.Repository.Entities;
using System.Reflection.Emit;

namespace PoemTown.Repository.Base;

public class PoemTownDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public PoemTownDbContext(DbContextOptions options) : base(options)
    {
    }
    public virtual DbSet<Announcement> Announcements => Set<Announcement>();
    public virtual DbSet<Collection> Collections => Set<Collection>();
    public virtual DbSet<Comment> Comments=> Set<Comment>();
    public virtual DbSet<CopyRight> CopyRights=> Set<CopyRight>();
    public virtual DbSet<Message> Messages=> Set<Message>();
    public virtual DbSet<Order> Orders => Set<Order>();
    public virtual DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public virtual DbSet<PaymentGateway> PaymentGateways => Set<PaymentGateway>();
    public virtual DbSet<Poem> Poems => Set<Poem>();
    public virtual DbSet<PoemHistory> PoemHistories => Set<PoemHistory>();
    public virtual DbSet<RecordFile> RecordFiles => Set<RecordFile>();
    public virtual DbSet<Report> Reports => Set<Report>();
    public virtual DbSet<TargetMark> TargetMarks => Set<TargetMark>();
    public virtual DbSet<Template> Templates => Set<Template>();
    public virtual DbSet<TemplateDetail> TemplateDetails => Set<TemplateDetail>();
    public virtual DbSet<Transaction> Transactions => Set<Transaction>();
    public virtual DbSet<UserCopyRight> UserCopyRights => Set<UserCopyRight>();
    public virtual DbSet<UserEWallet> UserEWallets => Set<UserEWallet>();
    public virtual DbSet<UserTemplate> UserTemplates => Set<UserTemplate>();



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


        // Transactions -> UserEWallets
        builder.Entity<Transaction>()
            .HasOne(t => t.EWallet)
            .WithMany(ew => ew.Transactions)
            .HasForeignKey(t => t.UserEWalletId)
            .OnDelete(DeleteBehavior.Restrict); // Thay Cascade bằng Restrict

        // Transactions -> Users
        builder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Thay Cascade bằng Restrict

        // Cấu hình quan hệ Poems -> Users
        builder.Entity<Poem>()
            .HasOne(p => p.User)
            .WithMany(u => u.Poems)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Thay Cascade bằng Restrict

        // Cấu hình quan hệ Poems -> Collections
        builder.Entity<Poem>()
            .HasOne(p => p.Collection)
            .WithMany(c => c.Poems)
            .HasForeignKey(p => p.CollectionId)
            .OnDelete(DeleteBehavior.Restrict); // Thay Cascade bằng Restrict

        // Quan hệ giữa Report và User (ReportUser)
        builder.Entity<Report>()
            .HasOne(r => r.ReportUser) // Một Report liên kết tới một User tạo báo cáo
            .WithMany(u => u.ReportUsers) // Một User có nhiều báo cáo đã tạo
            .HasForeignKey(r => r.ReportUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ giữa Report và User (ReportedUser)
        builder.Entity<Report>()
            .HasOne(r => r.ReportedUser) // Một Report liên kết tới một User bị báo cáo
            .WithMany(u => u.ReportedUsers) // Một User có thể bị báo cáo nhiều lần
            .HasForeignKey(r => r.ReportedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ giữa TargetMark và User (MarkByUser)
        builder.Entity<TargetMark>()
            .HasOne(tm => tm.MarkByUser) // Một TargetMark có một User đánh dấu
            .WithMany(u => u.MarkByUsers) // Một User có nhiều TargetMark đã đánh dấu
            .HasForeignKey(tm => tm.MarkByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ giữa TargetMark và User (MarkedUser)
        builder.Entity<TargetMark>()
            .HasOne(tm => tm.MarkedUser) // Một TargetMark có một User bị đánh dấu
            .WithMany(u => u.MarkedUsers) // Một User có thể bị đánh dấu nhiều lần
            .HasForeignKey(tm => tm.MarkedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ giữa Message và User (FromUser)
        builder.Entity<Message>()
            .HasOne(m => m.FromUser) // Một Message liên kết với một User gửi tin
            .WithMany(u => u.FromUser) // Một User có thể gửi nhiều Message
            .HasForeignKey(m => m.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ giữa Message và User (ToUser)
        builder.Entity<Message>()
            .HasOne(m => m.ToUser) // Một Message liên kết với một User nhận tin
            .WithMany(u => u.ToUser) // Một User có thể nhận nhiều Message
            .HasForeignKey(m => m.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<UserCopyRight>()
       .HasOne(uc => uc.CopyRight) // Một UserCopyRight liên kết với một CopyRight
       .WithMany(c => c.UserCopyRights) // Một CopyRight có nhiều UserCopyRight
       .HasForeignKey(uc => uc.CopyRightId) // Khóa ngoại trong UserCopyRight
       .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserCopyRight>()
        .HasOne(uc => uc.User) // Một UserCopyRight liên kết với một User
        .WithMany(u => u.UserCopyRights) // Một User có nhiều UserCopyRight
        .HasForeignKey(uc => uc.UserId) // Khóa ngoại trong UserCopyRight
        .OnDelete(DeleteBehavior.Restrict);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}