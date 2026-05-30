using Application.Users.DTOs;
using Application.Users.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace APIDazma.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthUseCase _authUseCase;

        public AuthController(AuthUseCase authUseCase)
        {
            _authUseCase = authUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var token = await _authUseCase.Login(dto.Identificador.Trim(), dto.Password.Trim());
            return Ok(new { token });   
        }
    }
}