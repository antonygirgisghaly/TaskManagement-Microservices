using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Application.Dtos
{
    public class UpdateTaskRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
    }
}
