using Domain.Entities;
using Domain.Ports;
using Domain.Exceptions;
using Domain.Entities.Business;

namespace Domain.Services.Users
{
    public class BusinessService
    {
        private readonly IBusinessRepository _businessRepository;

        public BusinessService(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }
        
        public async Task<BusinessEntity> CreateBusiness(BusinessEntity business)
        {
            if (await _businessRepository.ExistsByNit(business.NIT))
                throw new ValidationException($"La empresa con nit {business.NIT} ya está registrada");

            return await _businessRepository.CreateBusiness(business);
        }
    }
}
