using Application.Users.DTOs;
using Application.Users.UseCase;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace APIDazma.Controllers.Users
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class InvitationController : ControllerBase
    {
        private readonly GenerateInvitationCodeUseCase _invitationUseCase;
        private readonly JoinBusinessUseCase _joinBusinessUseCase;

        public InvitationController(GenerateInvitationCodeUseCase invitationUseCase, JoinBusinessUseCase joinBusinessUseCase)
        {
            _invitationUseCase = invitationUseCase;
            _joinBusinessUseCase = joinBusinessUseCase;
        }

        
        [HttpPost("generate")]
        [Authorize(Roles = "StoreAdmin")]
        public async Task<IActionResult> GenerateCode()
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var code = await _invitationUseCase.GenerateCode(int.Parse(authenticatedUserId)); 
            return Ok(new { code });
        }

        [HttpPost("join/{code}")]
        public async Task<IActionResult> JoinWithCode(string code)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var newToken = await _joinBusinessUseCase.JoinWithCode(code, int.Parse(authenticatedUserId));
            return Ok(new { message = "Te has unido exitosamente a la empresa.", token = newToken });
            
        }
    }
} 