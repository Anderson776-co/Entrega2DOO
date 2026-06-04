using Domain.Ports;
using Domain.Exceptions;
using Domain.Entities.Users;

namespace Domain.Services.Users
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRevokedTokenRepository _revokedTokenRepository;

        public AuthService(IUserRepository userRepository, IRevokedTokenRepository revokedTokenRepository)
        {
            _userRepository = userRepository;
            _revokedTokenRepository = revokedTokenRepository;
        }
        public async Task<UserEntity> Login(string identificador, string password)
        {
            var user = await _userRepository.Login(identificador, password);

            if (user == null)
                throw new UnauthorizedException("Credenciales incorrectas, favor revisar su email y contraseña");

            return user;
        }

        public async Task Logout(string jti, DateTime tokenExpiration)
        {
            await _revokedTokenRepository.RevokeAsync(jti, tokenExpiration);
        }
    }
}