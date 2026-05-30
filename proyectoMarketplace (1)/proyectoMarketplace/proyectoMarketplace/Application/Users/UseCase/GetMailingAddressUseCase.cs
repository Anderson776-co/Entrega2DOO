using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCase
{
    public class GetMailingAddressUseCase
    {
        private readonly MailingAddressService _mailingAddressService;

        public GetMailingAddressUseCase(MailingAddressService mailingAddressService)
        {
            _mailingAddressService = mailingAddressService;
        }   
        public async Task<List<MailingAddressDTO>> GetMailingAddresses(int idUser)
        {
            List<MailingAddressEntity> mailingAddressEntities =await _mailingAddressService.GetMailingAddresses(idUser);
            List<MailingAddressDTO> mailingAddressDTOs = new List<MailingAddressDTO>();
            foreach (var entity in mailingAddressEntities)
            {
                MailingAddressDTO dto = new MailingAddressDTO
                {
                    Address = entity.Address,
                    Department = entity.Department,
                    City = entity.City,
                    Neighborhood = entity.Neighborhood,
                    Complement = entity.Complement,
                    ContactPersonName = entity.ContactPersonName,
                    ContactPersonLastName = entity.ContactPersonLastName,
                    Phone = entity.Phone
                };
                mailingAddressDTOs.Add(dto);
            }
            return mailingAddressDTOs;
        }
    }
}
