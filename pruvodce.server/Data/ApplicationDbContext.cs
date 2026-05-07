using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using pruvodce.server.Models;

namespace pruvodce.server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventBuilding> EventBuildings { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<PointSubject> PointSubjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<AdminUser> AdminUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<EventBuilding>()
                .HasKey(eb => new { eb.EventId, eb.BuildingId });

            modelBuilder.Entity<EventBuilding>()
                .HasOne(eb => eb.Event)
                .WithMany(e => e.EventBuildings)
                .HasForeignKey(eb => eb.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasIndex(e => new { e.Name, e.StartDate })
                .IsUnique();

            modelBuilder.Entity<AdminUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Subject>()
                .HasIndex(s => s.Acronym)
                .IsUnique();

            modelBuilder.Entity<PointSubject>()
                .HasKey(ps => new { ps.PointId, ps.SubjectId });

            modelBuilder.Entity<PointSubject>()
                .HasOne(ps => ps.Point)
                .WithMany(p => p.PointSubjects)
                .HasForeignKey(ps => ps.PointId);

            modelBuilder.Entity<PointSubject>()
                .HasOne(ps => ps.Subject)
                .WithMany(s => s.PointSubjects)
                .HasForeignKey(ps => ps.SubjectId);
        }
    }
}