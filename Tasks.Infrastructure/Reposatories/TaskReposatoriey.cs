using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Application.Interfaces;
using Tasks.Domain.Entities;
using Tasks.Infrastructure.DbContexts;
namespace Tasks.Infrastructure.Reposatories
{
    public class TaskReposatoriey : ITaskReposatory
    {
        private readonly TasksDbContext _dbContext;
        public TaskReposatoriey(TasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TaskItem task, CancellationToken ct = default)
        {
             await _dbContext.Tasks.AddAsync(task,ct);
             await _dbContext.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(TaskItem task, CancellationToken ct = default)
        {
             _dbContext.Tasks.Remove(task); 
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<List<TaskItem>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.Tasks.Where(t => t.AssignedToUserId == userId).ToListAsync(ct);
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Tasks.FindAsync(id, ct);
        }

        public async Task UpdateAsync(TaskItem task, CancellationToken ct = default )
        {
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
