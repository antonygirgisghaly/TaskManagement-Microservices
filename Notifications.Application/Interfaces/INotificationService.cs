using Notifications.Application.Comman;
using Notifications.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Application.Interfaces
{
    public interface INotificationService
    {
        Task<Result<NotificationResponseDto>> CreateAsync(CreateNotificationRequestDto request, CancellationToken ct = default);
        Task<Result<List<NotificationResponseDto>>> GetMyNotificationsAsync(Guid userId, CancellationToken ct = default);
        Task<Result<bool>> MarkAsReadAsync(Guid notificationId, Guid userId, CancellationToken ct = default);
        Task<Result<bool>> DeleteAsync(Guid notificationId, Guid userId, CancellationToken ct = default);
    }
}
