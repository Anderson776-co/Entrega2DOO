using Domain.Entities.Users;
using Domain.Exceptions;
using Domain.Ports;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public void AssignBusiness(UserEntity user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public async Task<(bool emailExists, bool phoneExists)> ExistsDuplicate(string email, string phone)
        {
            var matches = await _context.Users
                .Where(u => u.Email == email || u.Phone == phone)
                .Select(u => new { u.Email, u.Phone })
                .ToListAsync();

            return (
                matches.Any(u => u.Email == email),
                matches.Any(u => u.Phone == phone)
            );
        }

        public async Task<UserEntity?> Login(string identificador, string password)
        {
            var autenticatedUser = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == identificador);

            if (autenticatedUser == null )
            {
                return null;
            }

            var passwordValid = Task.Run(() => BCrypt.Net.BCrypt.Verify(password, autenticatedUser.Password));

            if ( passwordValid == null)
            {
                return null;
            }
            return autenticatedUser;
        }
        public async Task<int?> GetBusinessIdByUserId(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.BusinessId)
                .FirstOrDefaultAsync();
        }

        public async Task<UserEntity> UpdateUser(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUser(UserEntity user)
        {
            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public Task<UserEntity?> GetUserById(int userId)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    } 
}
