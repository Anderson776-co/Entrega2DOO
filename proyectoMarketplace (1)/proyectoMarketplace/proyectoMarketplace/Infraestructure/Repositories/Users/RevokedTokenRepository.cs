using Domain.Entities.Users;
using Domain.Ports;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users
{
    public class RevokedTokenRepository : IRevokedTokenRepository
    {
        private readonly UserDbContext _context;

        public RevokedTokenRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task RevokeAsync(string jti, DateTime expiresAt)
        {
            _context.RevokedTokens.Add(new RevokedTokenEntity
            {
                Jti = jti,
                ExpiresAt = expiresAt,
                RevokedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsRevokedAsync(string jti)
        {
            return await _context.RevokedTokens
                .AnyAsync(t => t.Jti == jti);
        }

    }
}
