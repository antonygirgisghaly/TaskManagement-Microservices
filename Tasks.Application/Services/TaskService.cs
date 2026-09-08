using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Application.Comman;
using Tasks.Application.Dtos;
using Tasks.Application.Interfaces;
using Tasks.Domain.Entities;
using Tasks.Domain.Enums;

namespace Tasks.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskReposatory _taskRepository;
        private readonly INotificationClient _notificationClient;
        public TaskService(ITaskReposatory taskReposatory, INotificationClient notificationClient)
        {
            _taskRepository = taskReposatory;
            _notificationClient = notificationClient;
        }
        public async Task<Result<TaskResponseDto>> CreateAsync(CreateTaskRequestDto request, Guid userId, CancellationToken ct = default)
        {
            if (request.DueDate.HasValue && request.DueDate.Value <= DateTime.UtcNow)
                return Result<TaskResponseDto>.Failure("Due date must be in the future.");
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Status = TaskItemStatus.ToDo,
                AssignedToUserId = userId,
                CreatedAt = DateTime.UtcNow,
                DueDate = request.DueDate
            };
             await _taskRepository.AddAsync(task, ct);
             await _notificationClient.NotifyAsync(userId, $"Task '{task.Title}' has been created.", ct);
            return Result<TaskResponseDto>.Success(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedToUserId = task.AssignedToUserId,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate
            });
        }

        public async Task<Result<bool>> DeleteAsync(Guid taskId, Guid userId, CancellationToken ct = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, ct);
            if(task == null)
                return Result<bool>.Failure("Task not found");
            if(task.AssignedToUserId != userId)
                return Result<bool>.Failure("You are not authorized to delete this task.");
            await _taskRepository.DeleteAsync(task, ct);
            return Result<bool>.Success(true);
        }

        public async Task<Result<List<TaskResponseDto>>> GetMyTasksAsync(Guid userId, CancellationToken ct = default)
        {
            var tasks = await _taskRepository.GetAllByUserIdAsync(userId, ct);
            if (tasks is null)
                return Result<List<TaskResponseDto>>.Failure("No tasks found.");
            return Result<List<TaskResponseDto>>.Success(tasks.Select(task => new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedToUserId = task.AssignedToUserId,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate
            }).ToList());
        }
        public async Task<Result<TaskResponseDto>> GetTaskByIdAsync(Guid taskId, Guid userId, CancellationToken ct = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, ct);

            if (task is null)
                return Result<TaskResponseDto>.Failure("Task not found.");

            if (task.AssignedToUserId != userId)
                return Result<TaskResponseDto>.Failure("You are not authorized to view this task.");
            return Result<TaskResponseDto>.Success(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedToUserId = task.AssignedToUserId,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate
            });
        }
        public async Task<Result<TaskResponseDto>> UpdateStatusAsync(Guid taskId, UpdateTaskStatusRequestDto request, Guid userId, CancellationToken ct = default)
        {

            var task = await _taskRepository.GetByIdAsync(taskId,ct);

            if (task is null)
                return Result<TaskResponseDto>.Failure("Task not found.");

            if (task.AssignedToUserId != userId)
                return Result<TaskResponseDto>.Failure("You are not authorized to update this task.");

            task.Status = request.Status;
            await _taskRepository.UpdateAsync(task,ct);
            await _notificationClient.NotifyAsync(userId, $"Task '{task.Title}' status changed to {task.Status}.", ct);
            return Result<TaskResponseDto>.Success(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedToUserId = task.AssignedToUserId,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate
            });
        }
        public async Task<Result<TaskResponseDto>> UpdateAsync(Guid taskId, UpdateTaskRequestDto request, Guid userId, CancellationToken ct = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, ct);

            if (task is null)
                return Result<TaskResponseDto>.Failure("Task not found.");

            if (task.AssignedToUserId != userId)
                return Result<TaskResponseDto>.Failure("You are not authorized to update this task.");

            if (request.DueDate.HasValue && request.DueDate.Value <= DateTime.UtcNow)
                return Result<TaskResponseDto>.Failure("Due date must be in the future.");

            task.Title = request.Title;
            task.Description = request.Description;
            task.DueDate = request.DueDate;

            await _taskRepository.UpdateAsync(task, ct);

            return Result<TaskResponseDto>.Success(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                AssignedToUserId = task.AssignedToUserId,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate
            });
        }
    }
}
