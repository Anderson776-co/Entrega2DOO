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
            var duplicate = await _userRepository.ExistsDuplicate(user.Email, user.Username, user.Phone);

            var errors = new List<string>();
            if (duplicate.emailExists) errors.Add("Email already registered.");
            if (duplicate.usernameExists) errors.Add("Username already registered.");
            if (duplicate.phoneExists) errors.Add("Phone already registered.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" | ", errors));
            user.RoleId = 3;
            return await _userRepository.CreateUser(user);
        }
    }
}
