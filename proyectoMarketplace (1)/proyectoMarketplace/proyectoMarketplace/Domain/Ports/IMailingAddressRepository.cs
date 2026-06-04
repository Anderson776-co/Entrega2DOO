using Domain.Entities.Users;

namespace Domain.Ports
{
    public interface IMailingAddressRepository
    {
        Task<bool> ExistsAddressById(int userId, string address, int idAddress);
        Task<MailingAddressEntity> CreateAddress(MailingAddressEntity address);
        Task<List<MailingAddressEntity>> GetAddressesByUserId(int idUser);
        Task<bool> AddressExistsById(int idUser, int idAddress);

        Task<MailingAddressEntity?> GetAddressById(int idAddress);

        Task UpdateAddress(MailingAddressEntity address);
        Task DeleteAddress(MailingAddressEntity address);
    }
}
