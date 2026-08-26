using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Application.Comman;
using Tasks.Application.Dtos;

namespace Tasks.Application.Interfaces
{
    public interface ITaskService
    {
        Task<Result<TaskResponseDto>> CreateAsync(CreateTaskRequestDto request, Guid userId,CancellationToken ct = default);
        Task<Result<List<TaskResponseDto>>> GetMyTasksAsync(Guid userId,CancellationToken ct = default);
        Task<Result<TaskResponseDto>> GetTaskByIdAsync(Guid taskId, Guid userId, CancellationToken ct = default);
        Task<Result<TaskResponseDto>> UpdateStatusAsync(Guid taskId, UpdateTaskStatusRequestDto request, Guid userId,CancellationToken ct = default);
        Task<Result<TaskResponseDto>> UpdateAsync(Guid taskId, UpdateTaskRequestDto request, Guid userId, CancellationToken ct = default);
        Task<Result<bool>> DeleteAsync(Guid taskId, Guid userId, CancellationToken ct = default);
    }
}
