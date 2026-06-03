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

            if (!Validations.EsTelefonoValido(user.Phone))
                errors.Add("Formato de teléfono inválido. Debe contener 10 dígitos numericos.");

            if (user.Password != user.ConfirmPassword)
                errors.Add("Las contraseñas no coinciden.");

            if (errors.Count > 0)
                 throw new ValidationException(string.Join(" | ", errors));

            UserEntity userEntity = UserDTO.FromDTOtoEntity(user);
            UserEntity createdUser = await _userService.CreateUser(userEntity);
            return UserDTO.FromEntityToDTO(createdUser);
        }
    }
}
