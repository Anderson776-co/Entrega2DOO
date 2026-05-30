using Application.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Application.Users.UseCase;

namespace APIDazma.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BusinessController : ControllerBase
    {
        private readonly CreateBusinessUseCase _createBusinessUseCase;
        public BusinessController(CreateBusinessUseCase createBusinessUseCase)
        {
            _createBusinessUseCase = createBusinessUseCase;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBusiness(BusinessDTO dto)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var business = await _createBusinessUseCase.CreateBusiness(dto, int.Parse(authenticatedUserId));
            return Created("", new { message = "Business created successfully" });
            
        }
    }
}
