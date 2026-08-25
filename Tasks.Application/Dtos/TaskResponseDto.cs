using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Domain.Enums;

namespace Tasks.Application.Dtos
{
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public TaskItemStatus Status { get; set; }
        public Guid AssignedToUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
