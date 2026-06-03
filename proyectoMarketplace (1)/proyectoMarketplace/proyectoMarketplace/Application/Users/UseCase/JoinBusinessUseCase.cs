using Domain.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCase
{
    public class JoinBusinessUseCase
    {
        private readonly InvitationService _invitationService;

        public JoinBusinessUseCase(InvitationService invitationService)
        {
            _invitationService = invitationService;
        }
        public void JoinWithCode(string code, int userId)
        {
            _invitationService.JoinBusinessAsync(code, userId);
        }
    }
}
