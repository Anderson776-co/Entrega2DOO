using Application.Users.Interfaces;
using Domain.Exceptions;
using Domain.Services.Users;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Users.UseCase
{
    public class AuthUseCase
    {
        private readonly AuthService _authService;
        private readonly IjwtService _jwtService;

        public AuthUseCase(AuthService authService, IjwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        public async Task<string> Login(string identificador, string password)
        {
            if (!Validations.EsCorreo(identificador) && !Validations.EsUsername(identificador))
                throw new ValidationException("El identificador debe ser un correo electrónico o un nombre de usuario válido.");

            var user = await _authService.Login(identificador, password);
            return _jwtService.GenerateToken(user.Email, user.Role.Name, user.Id);
        }

        public async Task Logout(string token)
        {
            var handler = new JwtSecurityTokenHandler(); ;
            var jwtToken = handler.ReadJwtToken(token);

            await _authService.Logout(jwtToken.Id, jwtToken.ValidTo);
        }
    }
}
