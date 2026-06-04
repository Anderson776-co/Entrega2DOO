using Domain.Entities.Users;
using Domain.Exceptions;
using Domain.Ports;

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
            if (await _mailingAddressRepository.ExistsAddressById(address.UserId, address.Address, address.Id))
                throw new ConflictException($"La dirección {address.Address} ya existe para este usuario.");

            return await _mailingAddressRepository.CreateAddress(address);
        }

        public async Task UpdateMailingAddress(int idAddress, MailingAddressEntity mailingAddress, int userId)
        {
            var address = await _mailingAddressRepository.GetAddressById(idAddress);
            if (address == null)
                throw new NonFoundException("La dirección no existe.");

            if (address.UserId == userId)
                throw new ForbiddenException("No tienes permiso para actualizar esta dirección.");

            if (await _mailingAddressRepository.ExistsAddressById(address.UserId, mailingAddress.Address, address.Id))
                throw new ConflictException($"La dirección {mailingAddress.Address} ya existe para este usuario.");

            address.Address = mailingAddress.Address;
            address.Department = mailingAddress.Department;
            address.City = mailingAddress.City;
            address.Neighborhood = mailingAddress.Neighborhood;
            address.Complement = mailingAddress.Complement;

            await _mailingAddressRepository.UpdateAddress(address);
        }

        public async Task DeleteMailingAddress(int idAddress, int userId)
        {
            var address = await _mailingAddressRepository.GetAddressById(idAddress);
            if (address == null)   
                throw new NonFoundException("La dirección no existe.");
            if (address.UserId == userId)
                throw new ForbiddenException("No tienes permiso para eliminar esta dirección.");

            await _mailingAddressRepository.DeleteAddress(address);
        }

        public async Task<List<MailingAddressEntity>> GetMailingAddresses(int idUser)
        {
            return await _mailingAddressRepository.GetAddressesByUserId(idUser);
        }
    }
}
