using Domain.Entities.Users;
using Domain.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Users
{
    public class MailingAddressService
    {
        private readonly IMailingAddressRepository _mailingAddressRepository;

        public MailingAddressService(IMailingAddressRepository mailingAddressRepository)
        {
            _mailingAddressRepository = mailingAddressRepository;
        }

        public async Task<MailingAddressEntity> CreateAddress(MailingAddressEntity address)
        {
            if (await _mailingAddressRepository.ExistsAddressById(address.UserId, address.Address))
                throw new ValidationException("La dirección ya existe para este usuario.");

            return await _mailingAddressRepository.CreateAddress(address);
        }

        public async Task<List<MailingAddressEntity>> GetMailingAddresses(int idUser)
        {
            return await _mailingAddressRepository.GetAddressesByUserId(idUser);
        }
    }
}
