using Application.Users.DTO_s;
using Domain.Entities.Users;
using Domain.Exceptions;
using Domain.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Users.UseCase
{
    public class UpdatePersonalInformationUseCase
    {
        private readonly UserService _userService;

        public UpdatePersonalInformationUseCase(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePersonalInfoDTO> UpdateUser(int authenticatedUserId, UpdatePersonalInfoDTO user)
        {
            var errors = new List<string>();
            if (!Validations.NombreValido(user.Names, user.LastNames))
                errors.Add("Formato de nombre o apellido inválido. Solo se permiten letras y espacios.");

            if (!Validations.EsTelefonoValido(user.Phone))
                errors.Add("Número de teléfono inválido. Debe contener solo dígitos y tener una longitud de 10 caracteres.");

            if (!string.IsNullOrEmpty(user.Password) || !string.IsNullOrEmpty(user.ConfirmPassword))
            {
                if (user.Password != user.ConfirmPassword)
                {
                    errors.Add("Las contraseñas no coinciden");
                }
            }

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" | ", errors));
            
            UserEntity updatedUser = await _userService.UpdateUser(authenticatedUserId, user.Names, user.LastNames, user.Phone, user.Password);
            return UpdatePersonalInfoDTO.FromEntityToDTO(updatedUser);
        }
    }
}
