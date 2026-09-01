using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Application.Dtos
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = default!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
