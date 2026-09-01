using Microsoft.EntityFrameworkCore;
using Notifications.Application.Interfaces;
using Notifications.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Infrastructure.Reposatories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationsDbContext _context;

        public NotificationRepository(NotificationsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notfication>> GetAllByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<Notfication?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id, ct);
        }

        public async Task AddAsync(Notfication notification, CancellationToken ct = default)
        {
            await _context.Notifications.AddAsync(notification, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Notfication notification, CancellationToken ct = default)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Notfication notification, CancellationToken ct = default)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync(ct);
        }
    }
}
