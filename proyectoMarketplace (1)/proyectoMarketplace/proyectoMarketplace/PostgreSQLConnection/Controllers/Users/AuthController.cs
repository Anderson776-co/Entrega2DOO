using Application.Users.DTOs;
using Application.Users.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities.Zlib;

namespace APIDazma.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthUseCase _authUseCase;
        private readonly LogoutUseCase _logoutUseCase;

        public AuthController(AuthUseCase authUseCase, LogoutUseCase logoutUseCase)
        {
            _authUseCase = authUseCase;
            _logoutUseCase = logoutUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var token = await _authUseCase.Login(dto.Identificador.Trim(), dto.Password.Trim());
            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token no encontrado" });
            }

            await _logoutUseCase.Logout(token);
            return Ok(new { message = "Cierre de sesión exitoso. Por favor, elimine el token del lado del cliente." });
        }
    }
}