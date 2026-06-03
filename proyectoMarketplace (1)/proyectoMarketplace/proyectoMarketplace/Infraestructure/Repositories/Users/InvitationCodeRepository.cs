using Domain.Entities.Users;
using Domain.Ports;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Users
{
    public class InvitationCodeRepository : IInvitationCodeRepository
    {
        private readonly UserDbContext _context;

        public InvitationCodeRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateCode(InvitationCodeEntity invitationCode)
        {
            _context.InvitationCodes.Add(invitationCode);
            await _context.SaveChangesAsync();
            return invitationCode.Code;
        }

        public async Task<InvitationCodeEntity?> GetByCode(string code)
        {
            return await Task.FromResult(_context.InvitationCodes
                .FirstOrDefault(i => i.Code == code));
        }

        public async Task MarkAsUsed(InvitationCodeEntity invitationCode)
        {
            invitationCode.IsUsed = true;
            await _context.SaveChangesAsync();
        }
    }
}
