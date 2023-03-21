using DevTrack.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>, IApplicationDbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;
        private readonly ITimeService _timeService;

        public ApplicationDbContext(string connectionString, string migrationAssemblyName,
            ITimeService timeService)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
            _timeService = timeService;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = _timeService.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    b => b.MigrationsAssembly(_migrationAssemblyName)
                );
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>().HasKey(c => new { c.ApplicationUserId, c.ProjectId });

            modelBuilder.Entity<ProjectUser>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(n => n.ApplicationUserProjects)
                .HasForeignKey(x => x.ApplicationUserId);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(a => a.Project)
                .WithMany(n => n.ProjectApplicationUsers)
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<Activity>()
                .HasOne(n => n.MouseActivity)
                .WithOne(a => a.Activity)
                .HasForeignKey<MouseActivity>(x => x.ActivityId);

            modelBuilder.Entity<Activity>()
                .HasOne(n => n.ScreenCapture)
                .WithOne(a => a.Activity)
                .HasForeignKey<ScreenCapture>(x => x.ActivityId);

            modelBuilder.Entity<Activity>()
                .HasMany(n => n.ActiveWindows)
                .WithOne(a => a.Activity)
                .HasForeignKey(x => x.ActivityId);

            modelBuilder.Entity<Activity>()
                .HasOne(n => n.WebcamCapture)
                .WithOne(a => a.Activity)
                .HasForeignKey<WebcamCapture>(x => x.ActivityId);

            modelBuilder.Entity<Activity>()
                .HasMany(n => n.RunningPrograms)
                .WithOne(a => a.Activity)
                .HasForeignKey(x => x.ActivityId);

            modelBuilder.Entity<Activity>()
                .HasOne(n => n.KeyboardActivity)
                .WithOne(a => a.Activity)
                .HasForeignKey<KeyboardActivity>(x => x.ActivityId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(n => n.Activities)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey(x => x.ApplicationUserId);

            modelBuilder.Entity<Project>()
                .HasMany(n => n.Activities)
                .WithOne(a => a.Project)
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<Project>()
                .HasMany(n => n.Invitations)
                .WithOne(a => a.Project)
                .HasForeignKey(x => x.ProjectId);

            //Changing Invitation Entity "CreatedDate" ColumnName to "Date"
            modelBuilder.Entity<Invitation>()
                .Property(p => p.CreatedDate)
                .HasColumnName("Date");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ActiveWindow> ActiveWindows { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<KeyboardActivity> KeyboardActivities { get; set; }
        public DbSet<MouseActivity> MouseActivities { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<RunningProgram> RunningPrograms { get; set; }
        public DbSet<ScreenCapture> ScreenCaptures { get; set; }
        public DbSet<WebcamCapture> WebcamCaptures { get; set; }
    }
}