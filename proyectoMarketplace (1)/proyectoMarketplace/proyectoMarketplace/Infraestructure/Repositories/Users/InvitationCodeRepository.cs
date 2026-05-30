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

        public string CreateCode(InvitationCodeEntity invitationCode)
        {
            _context.InvitationCodes.Add(invitationCode);
            _context.SaveChanges();
            return invitationCode.Code;
        }

        public InvitationCodeEntity? GetByCode(string code)
        {
            return _context.InvitationCodes
                .FirstOrDefault(i => i.Code == code);
        }

        public void MarkAsUsed(InvitationCodeEntity invitationCode)
        {
            invitationCode.IsUsed = true;
            _context.SaveChanges();
        }
    }
}
