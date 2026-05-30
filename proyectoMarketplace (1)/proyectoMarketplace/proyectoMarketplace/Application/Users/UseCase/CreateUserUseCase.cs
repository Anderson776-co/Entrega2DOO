using Application;
using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Exceptions;
using Domain.Services.Users;

namespace Application.Users.UseCase
{
    public class CreateUserUseCase
    {
        private readonly UserService _userService;

        public CreateUserUseCase(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDTO> CreateUser(UserDTO user)
        {
            var errors = new List<string>();

            if (!Validations.NombreValido(user.Name, user.LastName))
              errors.Add("Formato de nombre o apellido inválido. Solo se permiten letras.");

            if (!Validations.EsCorreo(user.Email))
                errors.Add("El correo electrónico no es válido. Use un formato adecuado, por ejemplo: 'empresa@dominio.com', solo se permiten letras, números, puntos, guiones bajos y guiones antes del @.");

            if (!Validations.EsUsername(user.Username))
                //errors.Add("Formato de usuario inválido. Debe tener al menos 3 caracteres y puede contener letras, números, puntos, guiones bajos o guiones, y sin espacios.");

            if (!Validations.EsTelefonoValido(user.Phone))
                errors.Add("Formato de teléfono inválido. Debe contener 10 dígitos numericos.");

            if (errors.Count > 0)
                 throw new ValidationException(string.Join(" | ", errors));

            UserEntity userEntity = UserDTO.FromDTOtoEntity(user);
            UserEntity createdUser = await _userService.CreateUser(userEntity);
            return UserDTO.FromEntityToDTO(createdUser);
        }
    }
}
