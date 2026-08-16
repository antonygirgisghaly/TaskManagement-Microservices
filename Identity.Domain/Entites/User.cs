using Identity.Domain.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entites
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public int MyProperty { get; set; }
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
