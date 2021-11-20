using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace mesoBoard.Data
{
    public partial class mesoBoardContext : DbContext
    {
        public mesoBoardContext()
        {
        }

        public mesoBoardContext(DbContextOptions<mesoBoardContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<BBCode> BBCodes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<FileType> FileTypes { get; set; }
        public virtual DbSet<Forum> Forums { get; set; }
        public virtual DbSet<ForumPermission> ForumPermissions { get; set; }
        public virtual DbSet<InRole> InRoles { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<OnlineGuest> OnlineGuests { get; set; }
        public virtual DbSet<OnlineUser> OnlineUsers { get; set; }
        public virtual DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
        public virtual DbSet<Plugin> Plugins { get; set; }
        public virtual DbSet<PluginConfig> PluginConfigs { get; set; }
        public virtual DbSet<Poll> Polls { get; set; }
        public virtual DbSet<PollOption> PollOptions { get; set; }
        public virtual DbSet<PollVote> PollVotes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<ReportedPost> ReportedPosts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Smiley> Smilies { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }
        public virtual DbSet<ThreadView> ThreadViews { get; set; }
        public virtual DbSet<ThreadViewStamp> ThreadViewStamps { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(e => e.DownloadName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.SavedName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.PostID)
                    .HasConstraintName("FK_Attachments_Posts");
            });

            modelBuilder.Entity<BBCode>(entity =>
            {
                entity.Property(e => e.Parse).IsRequired();

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.Property(e => e.Group).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Note).HasMaxLength(100);

                entity.Property(e => e.Options).HasMaxLength(150);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<FileType>(entity =>
            {
                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(200);
            });

            modelBuilder.Entity<Forum>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Forums)
                    .HasForeignKey(d => d.CategoryID)
                    .HasConstraintName("FK_Forums_Categories");
            });

            modelBuilder.Entity<ForumPermission>(entity =>
            {
                entity.HasOne(d => d.Forum)
                    .WithMany(p => p.ForumPermissions)
                    .HasForeignKey(d => d.ForumID)
                    .HasConstraintName("FK_ForumPermissions_Forums");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ForumPermissions)
                    .HasForeignKey(d => d.RoleID)
                    .HasConstraintName("FK_Permissions_Roles");
            });

            modelBuilder.Entity<InRole>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.InRoles)
                    .HasForeignKey(d => d.RoleID)
                    .HasConstraintName("FK_InRoles_Roles");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InRoles)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_InRoles_Users");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.DateSent).HasColumnType("datetime");

                entity.Property(e => e.Subject).HasMaxLength(100);

                entity.Property(e => e.Text).IsRequired();

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.MessageFromUsers)
                    .HasForeignKey(d => d.FromUserID)
                    .HasConstraintName("FK_Messages_FromUser");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.MessageToUsers)
                    .HasForeignKey(d => d.ToUserID)
                    .HasConstraintName("FK_Messages_ToUser");
            });

            modelBuilder.Entity<OnlineGuest>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.IP)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<OnlineUser>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.Property(e => e.UserID).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.OnlineUser)
                    .HasForeignKey<OnlineUser>(d => d.UserID)
                    .HasConstraintName("FK_OnlineUsers_Users");
            });

            modelBuilder.Entity<PasswordResetRequest>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.Property(e => e.UserID).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.PasswordResetRequest)
                    .HasForeignKey<PasswordResetRequest>(d => d.UserID)
                    .HasConstraintName("FK_PasswordResetRequests_Users");
            });

            modelBuilder.Entity<Plugin>(entity =>
            {
                entity.Property(e => e.AssemblyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Author).HasMaxLength(50);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Version).HasMaxLength(50);

                entity.Property(e => e.Website).HasMaxLength(50);
            });

            modelBuilder.Entity<PluginConfig>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Note).HasMaxLength(100);

                entity.Property(e => e.Options).HasMaxLength(150);

                entity.Property(e => e.PluginGroup).HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Poll>(entity =>
            {
                entity.Property(e => e.PollID).ValueGeneratedNever();

                entity.Property(e => e.Question).IsRequired();

                entity.HasOne(d => d.PollNavigation)
                    .WithOne(p => p.Poll)
                    .HasForeignKey<Poll>(d => d.PollID)
                    .HasConstraintName("FK_Polls_Threads");
            });

            modelBuilder.Entity<PollOption>(entity =>
            {
                entity.Property(e => e.Text).IsRequired();

                entity.HasOne(d => d.Poll)
                    .WithMany(p => p.PollOptions)
                    .HasForeignKey(d => d.PollID)
                    .HasConstraintName("FK_PollOptions_Polls");
            });

            modelBuilder.Entity<PollVote>(entity =>
            {
                entity.HasOne(d => d.PollOption)
                    .WithMany(p => p.PollVotes)
                    .HasForeignKey(d => d.PollOptionID)
                    .HasConstraintName("FK_PollVotes_PollOptions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PollVotes)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_PollVotes_Users");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ParsedText).IsRequired();

                entity.Property(e => e.Text).IsRequired();

                entity.Property(e => e.TextOnly).IsRequired();

                entity.HasOne(d => d.Thread)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.ThreadID)
                    .HasConstraintName("FK_Posts_Threads");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_Posts_Users");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ReportedPost>(entity =>
            {
                entity.HasKey(e => e.PostID);

                entity.Property(e => e.PostID).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Post)
                    .WithOne(p => p.ReportedPost)
                    .HasForeignKey<ReportedPost>(d => d.PostID)
                    .HasConstraintName("FK_ReportedPosts_Posts");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.RankID)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Roles_Ranks");
            });

            modelBuilder.Entity<Smiley>(entity =>
            {
                entity.HasKey(e => e.SmileyID);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ImageURL)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasOne(d => d.Thread)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.ThreadID)
                    .HasConstraintName("FK_Subscriptions_Threads");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_Subscriptions_Users");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Thread>(entity =>
            {
                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.Forum)
                    .WithMany(p => p.Threads)
                    .HasForeignKey(d => d.ForumID)
                    .HasConstraintName("FK_Threads_Forums");
            });

            modelBuilder.Entity<ThreadView>(entity =>
            {
                entity.HasOne(d => d.Thread)
                    .WithMany(p => p.ThreadViews)
                    .HasForeignKey(d => d.ThreadID)
                    .HasConstraintName("FK_ThreadViews_Threads");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ThreadViews)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_ThreadViews_Users");
            });

            modelBuilder.Entity<ThreadViewStamp>(entity =>
            {
                entity.HasKey(e => e.ViewID);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Thread)
                    .WithMany(p => p.ThreadViewStamps)
                    .HasForeignKey(d => d.ThreadID)
                    .HasConstraintName("FK_ThreadViewStamp_Threads");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ThreadViewStamps)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_Views_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ActivationCode).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastLoginIP).HasMaxLength(50);

                entity.Property(e => e.LastLogoutDate).HasColumnType("datetime");

                entity.Property(e => e.LastPostDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.RegisterIP)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UsernameLower)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.Property(e => e.UserID).ValueGeneratedNever();

                entity.Property(e => e.AIM).HasMaxLength(200);

                entity.Property(e => e.Avatar).HasMaxLength(100);

                entity.Property(e => e.AvatarType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(100);

                entity.Property(e => e.MSN).HasMaxLength(200);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.DefaultRole)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserProfiles_Roles");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.ThemeID)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_UserProfiles_Themes");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserProfile)
                    .HasForeignKey<UserProfile>(d => d.UserID)
                    .HasConstraintName("FK_UserProfiles_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
