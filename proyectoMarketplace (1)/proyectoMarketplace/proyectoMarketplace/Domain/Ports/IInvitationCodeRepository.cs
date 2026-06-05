using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IInvitationCodeRepository
    {
        Task<string> CreateCode(InvitationCodeEntity invitationCode);
        Task<InvitationCodeEntity?> GetByCode(string code);
        Task MarkAsUsed(InvitationCodeEntity invitationCode);
        Task UpdateInvitation(InvitationCodeEntity invitationCode);
    }
}
