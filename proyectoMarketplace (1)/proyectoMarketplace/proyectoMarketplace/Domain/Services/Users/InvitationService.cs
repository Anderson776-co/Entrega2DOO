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
        public async Task<string> GenerateCode(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
                throw new NonFoundException("Su usuario no fue encontrado");

            if (user.BusinessId is null)
                throw new ValidationException("Debes crear una empresa antes de generar un código de invitación");

            var code = "DAZMA-" + GenerateRandomCode();

            var invitationCode = new InvitationCodeEntity
            {
                Code = code,
                BusinessId = user.BusinessId.Value,
                RoleId = user.RoleId,
                ExpiresAt = DateTime.Now.AddHours(24),
                IsUsed = false
            };

            return await _invitationRepository.CreateCode(invitationCode);
        }

        private string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(0, 5)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }

        public async Task JoinBusiness(string code, int userId)
        {
            var invitationCode = await _invitationRepository.GetByCode(code);

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
            await _invitationRepository.MarkAsUsed(invitationCode);
        }
    }
}
