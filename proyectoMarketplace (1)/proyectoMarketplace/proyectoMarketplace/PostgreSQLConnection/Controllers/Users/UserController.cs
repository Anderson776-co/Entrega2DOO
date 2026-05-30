
using Microsoft.AspNetCore.Mvc;
using Application.Users.DTOs;
using Application.Users.UseCase;
namespace APIDazma.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CreateUserUseCase _createUserUseCase;


        public UserController(CreateUserUseCase createUserUseCase)
        {
            _createUserUseCase = createUserUseCase;
        }

        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            var user = await _createUserUseCase.CreateUser(userDTO);
            return Created("", user);
        }
       
    }
}