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
            if (!Validations.EsCorreo(identificador))
                throw new ValidationException("Por favor, ingresa un correo electrónico válido. Ejemplo: nombre@dominio.com");

            var user = await _authService.Login(identificador, password);
            return _jwtService.GenerateToken(user.Email, user.Role.Name, user.Id);
        }
    }
}
