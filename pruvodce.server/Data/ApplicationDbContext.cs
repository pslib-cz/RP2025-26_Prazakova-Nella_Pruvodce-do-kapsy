using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using pruvodce.server.Models;

namespace pruvodce.server.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventBuilding> EventBuildings { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<EventPoint> EventPoints { get; set; }

        public DbSet<PointSubject> PointSubjects { get; set; }
        public DbSet<PointTeacher> PointTeachers { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<StudentNote> StudentNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .Property(e => e.Name)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .HasIndex(e => new { e.Name, e.StartDate })
                .IsUnique();

            modelBuilder.Entity<EventBuilding>()
                .HasKey(eb => new { eb.EventId, eb.BuildingId });

            modelBuilder.Entity<EventBuilding>()
                .HasOne(eb => eb.Event)
                .WithMany(e => e.EventBuildings)
                .HasForeignKey(eb => eb.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventPoint>()
                .HasKey(ep => new { ep.EventId, ep.PointId });

            modelBuilder.Entity<EventPoint>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventPoints)
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventPoint>()
                .HasOne(ep => ep.Point)
                .WithMany(p => p.EventPoints)
                .HasForeignKey(ep => ep.PointId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PointSubject>()
                .HasKey(ps => new { ps.PointId, ps.SubjectId });

            modelBuilder.Entity<PointSubject>()
                .HasOne(ps => ps.Point)
                .WithMany(p => p.PointSubjects)
                .HasForeignKey(ps => ps.PointId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PointSubject>()
                .HasOne(ps => ps.Subject)
                .WithMany(s => s.PointSubjects)
                .HasForeignKey(ps => ps.SubjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PointTeacher>()
                .HasKey(pt => pt.PointTeacherId);

            modelBuilder.Entity<PointTeacher>()
                .HasOne(pt => pt.Point)
                .WithMany(p => p.PointTeachers)
                .HasForeignKey(pt => pt.PointId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PointTeacher>()
                .HasOne(pt => pt.Teacher)
                .WithMany(t => t.PointTeachers)
                .HasForeignKey(pt => pt.TeacherId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subject>()
                .HasIndex(s => s.Acronym)
                .IsUnique();

            modelBuilder.Entity<Subject>()
                .HasOne(s => s.ActiveNote)
                .WithMany()
                .HasForeignKey(s => s.ActiveNoteStudentNoteId)
                .HasPrincipalKey(n => n.StudentNoteId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Notes)
                .WithOne(n => n.Subject)
                .HasForeignKey(n => n.SubjectId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Notes)
                .WithOne(n => n.Teacher)
                .HasForeignKey(n => n.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}