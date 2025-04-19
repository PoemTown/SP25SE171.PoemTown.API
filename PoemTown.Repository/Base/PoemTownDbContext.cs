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

    public virtual DbSet<Achievement> Achievements => Set<Achievement>();
    public virtual DbSet<Announcement> Announcements => Set<Announcement>();
    public virtual DbSet<Collection> Collections => Set<Collection>();
    public virtual DbSet<Comment> Comments=> Set<Comment>();
    public virtual DbSet<Follower> Followers => Set<Follower>();
    public virtual DbSet<LeaderBoard> LeaderBoards => Set<LeaderBoard>();
    public virtual DbSet<PoemLeaderBoard> PoemLeaderBoards => Set<PoemLeaderBoard>();
    public virtual DbSet<Like> Likes => Set<Like>();
    public virtual DbSet<MasterTemplate> MasterTemplates => Set<MasterTemplate>();
    public virtual DbSet<Message> Messages=> Set<Message>();
    public virtual DbSet<Order> Orders => Set<Order>();
    public virtual DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public virtual DbSet<PaymentGateway> PaymentGateways => Set<PaymentGateway>();
    public virtual DbSet<Poem> Poems => Set<Poem>();
    public virtual DbSet<PoemHistory> PoemHistories => Set<PoemHistory>();
    public virtual DbSet<RecordFile> RecordFiles => Set<RecordFile>();
    public virtual DbSet<Report> Reports => Set<Report>();
    public virtual DbSet<TargetMark> TargetMarks => Set<TargetMark>();
    public virtual DbSet<UserTemplate> UserTemplates => Set<UserTemplate>();
    public virtual DbSet<MasterTemplateDetail> MasterTemplateDetails => Set<MasterTemplateDetail>();
    public virtual DbSet<Transaction> Transactions => Set<Transaction>();
    /*public virtual DbSet<UserCopyRight> UserCopyRights => Set<UserCopyRight>();
    public virtual DbSet<UserCopyRightPoems> UserCopyRightPoems => Set<UserCopyRightPoems>();*/
    public virtual DbSet<UserEWallet> UserEWallets => Set<UserEWallet>();
    public virtual DbSet<UserLeaderBoard> UserLeaderBoards => Set<UserLeaderBoard>();
    public virtual DbSet<UsageRight> UsageRights => Set<UsageRight>();
    public virtual DbSet<UserTemplateDetail> UserTemplateDetails => Set<UserTemplateDetail>();
    public virtual DbSet<ThemeUserTemplateDetail> ThemeUserTemplateDetails => Set<ThemeUserTemplateDetail>();
    public virtual DbSet<Theme> Themes => Set<Theme>();
    public virtual DbSet<LoginTracking> LoginTrackings => Set<LoginTracking>();
    public virtual DbSet<SaleVersion> SaleVersions => Set<SaleVersion>();
    public virtual DbSet<PlagiarismPoemReport> PlagiarismPoemReports => Set<PlagiarismPoemReport>();
    public virtual DbSet<PoetSample> PoetSamples => Set<PoetSample>();
    public virtual DbSet<WithdrawalForm> WithdrawalForms => Set<WithdrawalForm>();
    public virtual DbSet<PoemType> PoemTypes => Set<PoemType>();
    public virtual DbSet<ReportMessage> ReportMessages => Set<ReportMessage>();
    public virtual DbSet<TitleSample> TitleSamples => Set<TitleSample>();
    public virtual DbSet<PoetSampleTitleSample> PoetSampleTitleSamples => Set<PoetSampleTitleSample>();
    /*
    public virtual DbSet<UserTemplate> UserTemplates => Set<UserTemplate>();
    */
    



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
        builder.Entity<LeaderBoard>().HasData(LeaderBoardDataSeeding.DefaultLeaderBoards);

        builder.Entity<UserToken>()
            .HasKey(ut => ut.Id);

        builder.Entity<UserToken>()
            .HasIndex(ut => ut.UserId)
            .IsUnique(false);  // Index to speed up queries, but no uniqueness

        
        // Transactions -> UserEWallets
        builder.Entity<Transaction>()
            .HasOne(t => t.UserEWallet)
            .WithMany(ew => ew.Transactions)
            .HasForeignKey(t => t.UserEWalletId)
            .OnDelete(DeleteBehavior.Restrict); // Thay Cascade bằng Restrict

        builder.Entity<Report>()
            .HasOne(p => p.Poem)
            .WithMany(r => r.Reports)
            .HasForeignKey(p => p.PoemId)
            .OnDelete(DeleteBehavior.Restrict); // Thay Cascade bằng Restrict
        
        /*// Transactions -> Users
        builder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Thay Cascade bằng Restrict*/

        /*builder.Entity<UserTemplateDetail>()
            .HasOne(ut => ut.ParentTemplateDetail)
            .WithMany()
            .HasForeignKey(ut => ut.ParentTemplateDetailId)
            .OnDelete(DeleteBehavior.Restrict);*/
        
        builder.Entity<ThemeUserTemplateDetail>()
            .HasOne(t => t.UserTemplateDetail)
            .WithMany(ut => ut.ThemeUserTemplateDetails) // Explicit inverse relationship
            .HasForeignKey(t => t.UserTemplateDetailId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        
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


        // Quan hệ giữa Role và UserRole
        builder.Entity<UserRole>()
            .HasOne(r => r.Role)
            .WithMany(ur => ur.UserRoles)
            .HasForeignKey(r => r.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Quan hệ giữa User và UserRole
        builder.Entity<UserRole>()
            .HasOne(u => u.User)
            .WithMany(ur => ur.UserRoles)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        /*builder.Entity<UserCopyRight>()
       .HasOne(uc => uc.Poem) // Một UserCopyRight liên kết với một CopyRight
       .WithMany(c => c.UserCopyRights) // Một CopyRight có nhiều UserCopyRight
       .HasForeignKey(uc => uc.PoemId) // Khóa ngoại trong UserCopyRight
       .OnDelete(DeleteBehavior.Restrict);*/

        /*builder.Entity<UserCopyRight>()
        .HasOne(uc => uc.User) // Một UserCopyRight liên kết với một User
        .WithMany(u => u.UserCopyRights) // Một User có nhiều UserCopyRight
        .HasForeignKey(uc => uc.UserId) // Khóa ngoại trong UserCopyRight
        .OnDelete(DeleteBehavior.Restrict);*/

        // Quan hệ giữa Follow và User (FollowUser)
        builder.Entity<Follower>()
            .HasOne(tm => tm.FollowUser) 
            .WithMany(u => u.FollowUser) 
            .HasForeignKey(tm => tm.FollowUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ giữa Follow và User (FollowedUser)
        builder.Entity<Follower>()
            .HasOne(tm => tm.FollowedUser) 
            .WithMany(u => u.FollowedUser) 
            .HasForeignKey(tm => tm.FollowedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        // Cấu hình tự tham chiếu ParentCommentId
        builder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.ChildComments)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction

        // Cấu hình quan hệ với User
        builder.Entity<Comment>()
            .HasOne(c => c.AuthorComment)
            .WithMany()
            .HasForeignKey(c => c.AuthorCommentId)
            .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction

        // Cấu hình quan hệ với Poem
        builder.Entity<Comment>()
            .HasOne(c => c.Poem)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PoemId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade có thể giữ nguyên

        /*builder.Entity<UserPoemRecordFile>()
       .HasOne(uc => uc.Poem) // Một UserCopyRight liên kết với một CopyRight
       .WithMany(c => c.UserPoemRecordFiles) // Một CopyRight có nhiều UserCopyRight
       .HasForeignKey(uc => uc.PoemId) // Khóa ngoại trong UserCopyRight
       .OnDelete(DeleteBehavior.Restrict);*/

        builder.Entity<UsageRight>()
        .HasOne(uc => uc.User) // Một UserCopyRight liên kết với một User
        .WithMany(u => u.UsageRights) // Một User có nhiều UserCopyRight
        .HasForeignKey(uc => uc.UserId) // Khóa ngoại trong UserCopyRight
        .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<UsageRight>()
        .HasOne(uc => uc.RecordFile) // Một UserCopyRight liên kết với một RecordFile
        .WithMany(r => r.UsageRights) // Một RecordFile có nhiều UserCopyRight
        .HasForeignKey(uc => uc.RecordFileId) // Khóa ngoại trong UserCopyRight
        .OnDelete(DeleteBehavior.Restrict);


        builder.Entity<RecordFile>()
       .HasOne(uc => uc.Poem) // Một UserCopyRight liên kết với một CopyRight
       .WithMany(c => c.RecordFiles) // Một CopyRight có nhiều UserCopyRight
       .HasForeignKey(uc => uc.PoemId) // Khóa ngoại trong UserCopyRight
       .OnDelete(DeleteBehavior.Restrict);

        /*builder.Entity<RecordFile>()
        .HasOne(uc => uc.User) // Một UserCopyRight liên kết với một User
        .WithMany(u => u.RecordFiles) // Một User có nhiều UserCopyRight
        .HasForeignKey(uc => uc.UserId) // Khóa ngoại trong UserCopyRight
        .OnDelete(DeleteBehavior.Restrict);*/

       builder.Entity<UserLeaderBoard>()
       .HasOne(uc => uc.LeaderBoard) // Một UserCopyRight liên kết với một CopyRight
       .WithMany(c => c.UserLeaderBoards) // Một CopyRight có nhiều UserCopyRight
       .HasForeignKey(uc => uc.LeaderBoardId) // Khóa ngoại trong UserCopyRight
       .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserLeaderBoard>()
        .HasOne(uc => uc.User) // Một UserCopyRight liên kết với một User
        .WithMany(u => u.UserLeaderBoards) // Một User có nhiều UserCopyRight
        .HasForeignKey(uc => uc.UserId) // Khóa ngoại trong UserCopyRight
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<LeaderBoard>()
        .HasMany(lb => lb.PoemLeaderBoards)
        .WithOne(d => d.LeaderBoard)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PoemLeaderBoard>()
        .HasOne(d => d.Poem)
        .WithMany(p => p.PoemLeaderBoards)
        .HasForeignKey(d => d.PoemId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Poem>()
            .HasOne(p => p.User)
            .WithMany(u => u.Poems)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RecordFile>()
            .HasOne(r => r.User)
            .WithMany(u => u.RecordFiles)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}