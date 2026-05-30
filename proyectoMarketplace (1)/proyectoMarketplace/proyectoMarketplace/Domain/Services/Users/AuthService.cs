using Domain.Ports;
using Domain.Exceptions;
using Domain.Entities.Users;

namespace Domain.Services.Users
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserEntity> Login(string identificador, string password)
        {

            var user = await _userRepository.Login(identificador, password);

            if (user == null)
                throw new UnauthorizedException("Invalid credentials");

            return user;
        }
    }
}