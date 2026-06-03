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
        private readonly UpdateMailingAddressUseCase _updateMailingAddressUse;
        private readonly DeleteMailingAddressUseCase _deleteMailingAddressUse;

        public MailingAddressController(CreateMailingAddressUseCase createMailingAddressUse, GetMailingAddressUseCase getMailingAddressUse, UpdateMailingAddressUseCase updateMailingAddressUse, DeleteMailingAddressUseCase deleteMailingAddressUse)
        {
            _createMailingAddressUse = createMailingAddressUse;
            _getMailingAddressUse = getMailingAddressUse;
            _updateMailingAddressUse = updateMailingAddressUse;
            _deleteMailingAddressUse = deleteMailingAddressUse;
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
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }
            var mailingAddresses = await _getMailingAddressUse.GetMailingAddresses(int.Parse(authenticatedUserId));
            return Ok(mailingAddresses);
        }

        [HttpPut("UpdateAddress/{idAddress}")]
        public async Task<IActionResult> UpdateMailingAddress(int idAddress, [FromBody] MailingAddressDTO dto)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            await _updateMailingAddressUse.UpdateMailingAddress(idAddress, dto, int.Parse(authenticatedUserId));
            return Ok(new { message = "La dirección de envío ha sido actualizada exitosamente" });
        }

        [HttpDelete("DeleteAddress/{id}")]
        public async Task<IActionResult> DeleteMailingAddress(int id)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            await _deleteMailingAddressUse.DeleteMailingAddress(id, int.Parse(authenticatedUserId));
            return Ok(new { message = "La dirección de envío ha sido eliminada exitosamente" });
        }
    }
}
