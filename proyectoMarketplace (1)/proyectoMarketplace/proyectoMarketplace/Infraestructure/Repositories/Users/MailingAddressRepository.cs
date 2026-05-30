using Domain.Entities.Users;
using Domain.Ports;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Users
{
    public class MailingAddressRepository : IMailingAddressRepository
    {
        private readonly UserDbContext _context;

        public MailingAddressRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAddressById(int idUser, string address)
        {
            return await Task.FromResult(_context.MailingAddresses.Any(a => a.UserId == idUser && a.Address == address.Replace(" ", "")));
        }

        public async Task<bool> AddressExistsById(int idUser, int idAddress)
        {
            return await Task.FromResult(_context.MailingAddresses.Any(a => a.UserId == idUser && a.Id == idAddress));
        }

        public async Task<MailingAddressEntity> CreateAddress(MailingAddressEntity address)
        {
            _context.MailingAddresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<List<MailingAddressEntity>> GetAddressesByUserId(int idUser)
        {
            return await Task.FromResult(_context.MailingAddresses.Where(a => a.UserId == idUser).ToList());
        }
    }
}
