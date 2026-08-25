using Microsoft.EntityFrameworkCore;
using Tasks.Domain.Entities;

namespace Tasks.Infrastructure.DbContexts
{
    public class TasksDbContext : DbContext
    {
        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TasksDbContext).Assembly);
        }
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
    }
}
