using Domain.Exceptions;
using Domain.Ports;
using Domain.Entities.Users;

namespace Domain.Services.Users
{
    public class InvitationService
    {
        private readonly IInvitationCodeRepository _invitationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBusinessRepository _businessRepository;

        public InvitationService(IInvitationCodeRepository invitationRepository, IUserRepository userRepository, IBusinessRepository businessRepository)
        {
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
            _businessRepository = businessRepository;
        }
        public string GenerateCode(int businessId, int roleId)
        {
            var code = "DAZMA-" + GenerateRandomCode();

            var invitationCode = new InvitationCodeEntity
            {
                Code = code,
                BusinessId = businessId,
                RoleId = roleId,
                ExpiresAt = DateTime.Now.AddHours(24),
                IsUsed = false
            };

            return _invitationRepository.CreateCode(invitationCode);
        }

        private string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(0, 5)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }

        public async Task JoinBusinessAsync(string code, int userId)
        {
            var invitationCode = _invitationRepository.GetByCode(code);

            if (invitationCode == null)
                throw new NonFoundException("Codigo de invitacion no encontrado");

            if (invitationCode.IsUsed)
                throw new ValidationException("El codigo de invitacion ya ha sido utilizado");

            if (invitationCode.ExpiresAt < DateTime.Now)
                throw new ValidationException("El codigo de invitacion ha expirado");

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                throw new NonFoundException("User not found");

            user.BusinessId = invitationCode.BusinessId;
            user.RoleId = invitationCode.RoleId;

            _userRepository.AssignBusiness(user);
            _invitationRepository.MarkAsUsed(invitationCode);
        }
    }
}
