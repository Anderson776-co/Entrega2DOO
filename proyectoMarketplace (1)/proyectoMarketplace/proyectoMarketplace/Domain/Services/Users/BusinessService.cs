using Domain.Ports;
using Domain.Exceptions;
using Domain.Entities.Business;
using Domain.Models;

namespace Domain.Services.Users
{
    public class BusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserRepository _userRepository;

        public BusinessService(IBusinessRepository businessRepository, IUserRepository userRepository)
        {
            _businessRepository = businessRepository;
            _userRepository = userRepository;
        }

        public async Task<BusinessEntity> CreateBusiness(BusinessEntity business, int adminUserId)
        {
            var adminUser = await _userRepository.GetUserById(adminUserId);
            if (adminUser == null)
                throw new NonFoundException("No se encontró el usuario administrador");

            if (await _businessRepository.ExistsByNit(business.NIT))
                throw new ValidationException($"La empresa con nit {business.NIT} ya está registrada");

            var createdBusiness = await _businessRepository.CreateBusiness(business);

            adminUser.RoleId = 1;
            adminUser.RegisterType = RegisterType.Business;
            adminUser.BusinessId = createdBusiness.Id;

            await _userRepository.UpdateUser(adminUser);

            return createdBusiness;
        }

        public async Task<BusinessEntity> GetBusinessInfo(int userId)
        {
            int? businessId = await _userRepository.GetBusinessIdByUserId(userId);

            if (businessId is null)
                throw new NonFoundException("No se encontró una empresa registrada asociada a su usuario");

            var business = await _businessRepository.GetBusinessById(businessId.Value);

            if (business == null)
            {
                throw new NonFoundException("No se encontró el registro de empresa de su usuario");
            }

            return business;
        }
    }
}
