using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Domain.Enums;

namespace Tasks.Application.Dtos
{
    public class UpdateTaskStatusRequestDto
    {
        public TaskItemStatus Status { get; set; }
    }
}
