using Application.Users.DTO_s.ResponseDTO;
using Domain.Entities.Users;
using Domain.Services.Users;

namespace Application.Users.UseCase
{
    public class GetMailingAddressUseCase
    {
        private readonly MailingAddressService _mailingAddressService;

        public GetMailingAddressUseCase(MailingAddressService mailingAddressService)
        {
            _mailingAddressService = mailingAddressService;
        }   
        public async Task<List<AddressResponseDTO>> GetMailingAddresses(int idUser)
        {
            List<MailingAddressEntity> mailingAddressEntities =await _mailingAddressService.GetMailingAddresses(idUser);
            List<AddressResponseDTO> mailingAddressDTOs = new List<AddressResponseDTO>();

            foreach (var entity in mailingAddressEntities)
            {
                AddressResponseDTO dto = AddressResponseDTO.FromEntityToDTO(entity);
                mailingAddressDTOs.Add(dto);
            }

            return mailingAddressDTOs;
        }
    }
}
