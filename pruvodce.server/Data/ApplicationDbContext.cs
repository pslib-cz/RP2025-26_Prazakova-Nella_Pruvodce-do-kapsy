using Microsoft.EntityFrameworkCore;
using pruvodce.server.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace pruvodce.server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //test data
            DbInitializer.Seed(modelBuilder);
        }
    }
}