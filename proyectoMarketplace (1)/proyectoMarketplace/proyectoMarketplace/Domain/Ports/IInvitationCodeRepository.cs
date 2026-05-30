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
        public string CreateCode(InvitationCodeEntity invitationCode);
        public InvitationCodeEntity? GetByCode(string code);
        public void MarkAsUsed(InvitationCodeEntity invitationCode);
    }
}
