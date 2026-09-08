using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Application.Interfaces
{
    public interface INotificationClient
    {
        Task NotifyAsync(Guid userId, string message, CancellationToken ct = default);
    }
}
