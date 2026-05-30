using Application.Users.DTOs;
using Application.Users.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace APIDazma.Controllers.Users
{
    [ApiController]
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

        [Authorize]
        [HttpPost("generate/{nit}/{roleId}")]
        public IActionResult GenerateCode(string nit, int roleId)
        {
           var code = _invitationUseCase.GenerateCode(nit, roleId);
           return Ok(new { code });
        }

        [HttpPost("join")]
        public IActionResult JoinWithCode([FromBody] JoinRequestDTO dto)
        {
            _joinBusinessUseCase.JoinWithCode(dto.Code, dto.UserId);
            return Ok(new { message = "Te has unido exitosamente a la empresa." });
            
        }
    }
} 