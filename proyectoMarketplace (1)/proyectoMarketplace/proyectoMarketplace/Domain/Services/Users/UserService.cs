using Domain.Models;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Entities.Users;
namespace Domain.Services.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            var duplicate = await _userRepository.ExistsDuplicate(user.Email, user.Phone);

            var errors = new List<string>();
            if (duplicate.emailExists) errors.Add($"El correo {user.Email} ya está registrado.");
            if (duplicate.phoneExists) errors.Add($"El teléfono {user.Phone} ya está registrado.");

            if (errors.Count > 0)
                throw new ConflictException(string.Join(" | ", errors));

            user.RoleId = 3;
            user.IsActive = true;

            return await _userRepository.CreateUser(user);
        }

        public async Task<UserEntity> UpdateUser(int idUser, string names, string lastNames, string phone, string? password)
        {
            var existingUser = await _userRepository.GetUserById(idUser);
            if (existingUser == null)
                throw new NonFoundException("Usuario no encontrado");

            existingUser.Name = names;
            existingUser.LastName = lastNames;
            existingUser.Phone = phone;

            if (!string.IsNullOrEmpty(password))
                existingUser.Password = await _userRepository.ChangePassword(password);

            return await _userRepository.UpdateUser(existingUser);
        }

        public async Task<UserEntity> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                throw new NonFoundException("Usuario no encontrado");
            return user;
        }

        public async Task DeleteUser(int id)
        {
            var existingUser = await _userRepository.GetUserById(id);

            if (existingUser == null)
                throw new NonFoundException("Usuario no encontrado");

            await _userRepository.DeleteUser(existingUser);
        }
    }
}
