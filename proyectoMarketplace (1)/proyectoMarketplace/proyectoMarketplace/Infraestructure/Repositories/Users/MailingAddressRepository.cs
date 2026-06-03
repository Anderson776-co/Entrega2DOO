using Domain.Entities.Users;
using Domain.Ports;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

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

        public Task<MailingAddressEntity?> GetAddressById(int idAddress, int userId)
        {
            return  _context.MailingAddresses.FirstOrDefaultAsync(a => a.Id == idAddress && a.UserId == userId);
        }

        public async Task UpdateAddress(MailingAddressEntity address)
        {
            _context.MailingAddresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddress(MailingAddressEntity address)
        {
            _context.MailingAddresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}
