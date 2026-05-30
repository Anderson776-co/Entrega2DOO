using Domain.Entities.Users;

namespace Domain.Ports
{
    public interface IMailingAddressRepository
    {
        Task<bool> ExistsAddressById(int id, string address);
        Task<MailingAddressEntity> CreateAddress(MailingAddressEntity address);
        Task<List<MailingAddressEntity>> GetAddressesByUserId(int idUser);
        Task<bool> AddressExistsById(int idUser, int idAddress);
    }
}
