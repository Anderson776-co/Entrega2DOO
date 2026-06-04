using Application.Users.Interfaces;
using Domain.Services.Users;

namespace Application.Users.UseCase
{
    public class JoinBusinessUseCase
    {
        private readonly InvitationService _invitationService;
        private UserService _userService;
        private readonly IjwtService _jwtService;


        public JoinBusinessUseCase(InvitationService invitationService, UserService userService, IjwtService jwtService)
        {
            _invitationService = invitationService;
            _userService = userService;
            _jwtService = jwtService;
        }
        public async Task<string> JoinWithCode(string code, int userId)
        {
            await _invitationService.JoinBusiness(code, userId);

            var user = await _userService.GetUserById(userId);
            return _jwtService.GenerateToken(user.Email, user.Role.Name, user.Id);
        }
    }
}
