
using Application.Users.DTO_s;
using Application.Users.DTOs;
using Application.Users.UseCase;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace APIDazma.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CreateUserUseCase _createUserUseCase;
        private readonly UpdatePersonalInformationUseCase _UpdatePersonalInformationUseCase;
        private readonly GetPersonalInfoUseCase _getPersonalInfoUseCase;
        private readonly deleteUserAccountUseCase _deleteUserAccountUseCase;
        private readonly AuthUseCase _authUseCase;


        public UserController(CreateUserUseCase createUserUseCase, UpdatePersonalInformationUseCase updatePersonalInformationUseCase, GetPersonalInfoUseCase getPersonalInfoUseCase, deleteUserAccountUseCase deleteUserAccountUseCase, AuthUseCase authUseCase)
        {
            _createUserUseCase = createUserUseCase;
            _UpdatePersonalInformationUseCase = updatePersonalInformationUseCase;
            _getPersonalInfoUseCase = getPersonalInfoUseCase;
            _deleteUserAccountUseCase = deleteUserAccountUseCase;
            _authUseCase = authUseCase;
        }

        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            var user = await _createUserUseCase.CreateUser(userDTO);
            return Created("", user);
        }

        [Authorize]
        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUser(UpdatePersonalInfoDTO userDTO)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var user = await _UpdatePersonalInformationUseCase.UpdateUser(int.Parse(authenticatedUserId), userDTO);
            return Ok(user);
        }

        [Authorize]
        [HttpGet("getPersonalInfo")]
        public async Task<IActionResult> GetPersonalInfo()
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var user = await _getPersonalInfoUseCase.GetPersonalInfo(int.Parse(authenticatedUserId));
            return Ok(user);
        }

        [Authorize]
        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUser()
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
            {
                throw new UnauthorizedException("El usuario no esta autenticado");
            }

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await _authUseCase.Logout(token);

            await _deleteUserAccountUseCase.DeleteUserAccount(int.Parse(authenticatedUserId));

            return Ok();
        }
    }
}