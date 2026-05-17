using Microsoft.EntityFrameworkCore;
using TaskMgmt.Model;

namespace TaskMgmt.Data
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

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Title = "Set up project structure", Description = "Create folders and configure dependencies.", IsCompleted = true },
                new TaskItem { Id = 2, Title = "Design database schema", Description = "Plan tables, relationships and migrations.", IsCompleted = true },
                new TaskItem { Id = 3, Title = "Build REST API endpoints", Description = "Implement GET, POST, PUT, DELETE for tasks.", IsCompleted = false },
                new TaskItem { Id = 4, Title = "Integrate Angular frontend", Description = "Connect UI to the API and handle responses.", IsCompleted = false },
                new TaskItem { Id = 5, Title = "Deploy to production", Description = "Configure hosting and set up CI/CD pipeline.", IsCompleted = false }
            );
        }        
    }
}
