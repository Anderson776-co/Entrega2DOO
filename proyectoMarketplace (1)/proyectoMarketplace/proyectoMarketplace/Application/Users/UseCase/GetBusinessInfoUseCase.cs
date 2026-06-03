
using Application.Users.DTOs;
using Domain.Services.Users;

namespace Application.Users.UseCase
{
    public class GetBusinessInfoUseCase
    {
        private readonly BusinessService _businessService;

        public GetBusinessInfoUseCase(BusinessService businessService)
        {
            _businessService = businessService;
        }

        public async Task<BusinessDTO> GetBusinessInfo(int userId)
        {
            var businessInfo = await _businessService.GetBusinessInfo(userId);
            return BusinessDTO.FromEntityToDTO(businessInfo);
        }
    }
}
