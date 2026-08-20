using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.DTOs
{
    public class AuthResponseDto
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
