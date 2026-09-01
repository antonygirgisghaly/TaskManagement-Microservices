using Notifications.Application.Comman;
using Notifications.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notfication>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<Notfication?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Notfication notification, CancellationToken ct = default);
        Task UpdateAsync(Notfication notification, CancellationToken ct = default);
        Task DeleteAsync(Notfication notification, CancellationToken ct = default);
    }
}
    

