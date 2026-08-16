using Identity.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Comman
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddUserAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task<bool> ExistsUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
