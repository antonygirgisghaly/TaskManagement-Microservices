using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Domain.Enums;

namespace Tasks.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public TaskItemStatus Status { get; set; } = TaskItemStatus.ToDo;
        public Guid AssignedToUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
    }
}
