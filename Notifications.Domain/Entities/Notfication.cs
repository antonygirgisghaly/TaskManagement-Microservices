using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Domain.Entities
{
    public class Notfication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = default!;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
