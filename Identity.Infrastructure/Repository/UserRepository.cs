
using Identity.Domain.Comman;
using Identity.Domain.Entites;
using Microsoft.EntityFrameworkCore;
namespace Identity.Infrastructure.Repository
{
    public class UserRepository :IUserRepository
    {
        private readonly IdentityDbContext _context;
        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
          => await _context.Users.AddAsync(user, cancellationToken);

        public async Task<bool> ExistsUserByEmailAsync(string email, CancellationToken cancellationToken = default)
         => await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
         => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
