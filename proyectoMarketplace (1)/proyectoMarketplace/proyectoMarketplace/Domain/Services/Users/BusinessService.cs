using Domain.Entities;
using Domain.Ports;
using Domain.Exceptions;
using Domain.Entities.Business;

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

        public async Task<BusinessEntity> CreateBusiness(BusinessEntity business)
        {
            var adminUser = await _userRepository.GetUserById(business.AdminUserId);
            if (adminUser == null)
            {
                throw new NonFoundException($"No se encontró registros de su usuario");
            }
            adminUser.RoleId = 1; // Asignar el rol de administrador al usuario

            if (await _businessRepository.ExistsByNit(business.NIT))
                throw new ValidationException($"La empresa con nit {business.NIT} ya está registrada");

            return await _businessRepository.CreateBusiness(business);
        }

        public async Task<BusinessEntity> GetBusinessInfo(int userId)
        {
            int? businessId = await _userRepository.GetBusinessIdByUserId(userId);

            if (businessId is null)
                throw new NonFoundException($"No se encontró un negocio para su usuario");

            var business = await _businessRepository.GetBusinessById(businessId.Value);
            if (business == null)
            {
                throw new NonFoundException($"No se encontró un negocio registrado para su usuario");
            }

            return business;
        }
    }
}
