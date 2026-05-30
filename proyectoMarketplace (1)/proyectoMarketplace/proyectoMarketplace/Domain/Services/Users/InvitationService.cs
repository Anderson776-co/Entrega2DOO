using Domain.Entities;
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
        public string GenerateCode(string nit, int roleId)
        {
            var business = _businessRepository.GetBusinessByNit(nit);
            if (business == null)
                throw new ValidationException("Empresa no encontrada");

            var code = "DAZMA-" + GenerateRandomCode();

            var invitationCode = new InvitationCodeEntity
            {
                Code = code,
                BusinessId = business.Id,
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

        public void JoinBusiness(string code, int userId)
        {
            var invitationCode = _invitationRepository.GetByCode(code);

            if (invitationCode == null)
                throw new ValidationException("Invalid invitation code");

            if (invitationCode.IsUsed)
                throw new ValidationException("Invitation code has already been used");

            if (invitationCode.ExpiresAt < DateTime.Now)
                throw new ValidationException("Invitation code has expired");

            _userRepository.AssignBusiness(userId, invitationCode.BusinessId, invitationCode.RoleId);
            _invitationRepository.MarkAsUsed(invitationCode);
        }
    }
}
