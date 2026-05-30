using Application.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Exceptions;
using Application.Users.UseCase;

namespace APIDazma.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MailingAddressController : ControllerBase
    {
        private readonly CreateMailingAddressUseCase _createMailingAddressUse;
        private readonly GetMailingAddressUseCase _getMailingAddressUse;

        public MailingAddressController(CreateMailingAddressUseCase createMailingAddressUse, GetMailingAddressUseCase getMailingAddressUse)
        {
            _createMailingAddressUse = createMailingAddressUse;
            _getMailingAddressUse = getMailingAddressUse;  
        }

        [HttpPost("CreateAddress")]
        public async Task<IActionResult> CreateShoppingAddress([FromBody] MailingAddressDTO dto)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var shoppingAddress =await _createMailingAddressUse.CreateMailingAddress(dto, int.Parse(authenticatedUserId));
            return Created("", new { message = "Shopping address created successfully"});
        }

        [HttpGet("GetUserAddresses")]
        public async Task<IActionResult> GetShoppingAddresses()
        {
            var authenticatedUsername = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (authenticatedUsername == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }
            var shoppingAddresses = await _getMailingAddressUse.GetMailingAddresses(int.Parse(authenticatedUserId));
            return Ok(shoppingAddresses);
        }
    }
}
