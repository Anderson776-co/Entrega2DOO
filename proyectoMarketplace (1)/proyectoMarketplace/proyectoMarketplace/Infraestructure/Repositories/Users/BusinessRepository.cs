using Domain.Entities.Business;
using Domain.Ports;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Users
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly UserDbContext _context;

        public BusinessRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessEntity> CreateBusiness(BusinessEntity business)
        {
            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<BusinessEntity?> GetBusinessByNit(string nit)
        {
            return await _context.Businesses.FirstOrDefaultAsync(b => b.NIT == nit);
        }

        public async Task<bool> ExistsById(int id)
        {
            return await _context.Businesses.AnyAsync(b => b.Id == id);
        }

        public async Task<bool> ExistsByNit(string nit)
        {
            return await _context.Businesses.AnyAsync(b => b.NIT == nit);
        }
    }
}