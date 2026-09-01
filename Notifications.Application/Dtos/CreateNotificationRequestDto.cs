using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Application.Dtos
{
    public class CreateNotificationRequestDto
    {
        public Guid UserId { get; set; }
        public string Message { get; set; } = default!;
    }
}
