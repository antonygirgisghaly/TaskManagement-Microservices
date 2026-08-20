using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.DTOs
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
