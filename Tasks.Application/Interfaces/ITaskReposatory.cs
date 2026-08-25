using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Domain.Entities;

namespace Tasks.Application.Interfaces
{
    public interface ITaskReposatory
    {
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<TaskItem>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task AddAsync(TaskItem task, CancellationToken ct = default);
        Task UpdateAsync(TaskItem task, CancellationToken ct = default);
        Task DeleteAsync(TaskItem task, CancellationToken ct = default);
    }
}
