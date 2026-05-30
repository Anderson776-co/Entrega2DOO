using Domain.Entities.Users;
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
        public void AssignBusiness(int userId, int businessId, int roleId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new ValidationException("User not found");

            user.BusinessId = businessId;
            user.RoleId = roleId;
            _context.SaveChanges();
        }
        public UserEntity? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public async Task<(bool emailExists, bool usernameExists, bool phoneExists)> ExistsDuplicate(string email, string username, string phone)
        {
            var matches = await _context.Users
                .Where(u => u.Email == email || u.Username == username || u.Phone == phone)
                .Select(u => new { u.Email, u.Username, u.Phone })
                .ToListAsync();

            return (
                matches.Any(u => u.Email == email),
                matches.Any(u => u.Username == username),
                matches.Any(u => u.Phone == phone)
            );
        }
        public UserEntity? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
        /*public Role GetRoleById(int roleId)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == roleId);
        }*/
        public UserEntity? GetByUsernameWithRole(string username)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == username);
        }

        public async Task<UserEntity?> Login(string identificador, string password)
        {
            var autenticatedUser = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == identificador || u.Email == identificador);

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
        public int? GetBusinessIdByUserId(int userId)
        {
            return _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.BusinessId)
                .FirstOrDefault();
        }
    } 
}
