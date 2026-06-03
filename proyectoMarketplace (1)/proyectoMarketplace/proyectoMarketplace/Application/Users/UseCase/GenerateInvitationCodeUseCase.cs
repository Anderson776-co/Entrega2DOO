using Domain.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCase
{
    public class GenerateInvitationCodeUseCase
    {
        private readonly InvitationService _invitationService;

        public GenerateInvitationCodeUseCase(InvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        public async Task<string> GenerateCode(int userId)
        {
            var invitation = await _invitationService.GenerateCode(userId);
            return invitation;
        }
    }
}
