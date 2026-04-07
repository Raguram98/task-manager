using Microsoft.EntityFrameworkCore;

namespace TaskMgmt
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
