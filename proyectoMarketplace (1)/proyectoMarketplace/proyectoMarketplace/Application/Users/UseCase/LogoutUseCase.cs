using Domain.Services.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UseCase
{
    public class LogoutUseCase
    {
        private readonly AuthService _authService;

        public LogoutUseCase(AuthService authService)
        {
            _authService = authService;
        }
        public async Task Logout(string token)
        {
            var handler = new JwtSecurityTokenHandler(); ;
            var jwtToken = handler.ReadJwtToken(token);

            await _authService.Logout(jwtToken.Id, jwtToken.ValidTo);
        }
    }
}
