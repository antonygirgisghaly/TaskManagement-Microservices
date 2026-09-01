using Notifications.Application.Comman;
using Notifications.Application.Dtos;
using Notifications.Application.Interfaces;
using Notifications.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Application.Services
{
    public class NotficationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotficationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Result<NotificationResponseDto>> CreateAsync(CreateNotificationRequestDto request, CancellationToken ct = default)
        {
            var notification = new Notfication
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Message = request.Message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification, ct);

            return Result<NotificationResponseDto>.Success(MapToDto(notification));
        }

        public async Task<Result<List<NotificationResponseDto>>> GetMyNotificationsAsync(Guid userId, CancellationToken ct = default)
        {
            var notifications = await _notificationRepository.GetAllByUserIdAsync(userId, ct);

            return Result<List<NotificationResponseDto>>.Success(notifications.Select(MapToDto).ToList());
        }

        public async Task<Result<bool>> MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId, ct);

            if (notification is null)
                return Result<bool>.Failure("Notification not found.");

            if (notification.UserId != userId)
                return Result<bool>.Failure("You are not authorized to update this notification.");

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification, ct);

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(Guid notificationId, Guid userId, CancellationToken ct = default)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId, ct);

            if (notification is null)
                return Result<bool>.Failure("Notification not found.");

            if (notification.UserId != userId)
                return Result<bool>.Failure("You are not authorized to delete this notification.");

            await _notificationRepository.DeleteAsync(notification, ct);

            return Result<bool>.Success(true);
        }

        private static NotificationResponseDto MapToDto(Notfication notification)
        {
            return new NotificationResponseDto
            {
                Id = notification.Id,
                Message = notification.Message,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}
