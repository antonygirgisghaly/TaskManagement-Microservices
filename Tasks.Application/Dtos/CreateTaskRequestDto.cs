using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.Application.Dtos
{
    public class CreateTaskRequestDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime? DueDate { get; set; }
    }
}
