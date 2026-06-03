using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Exceptions;
using Domain.Services.Users;

namespace Application.Users.UseCase
{
    public class UpdateMailingAddressUseCase
    {
        private readonly MailingAddressService  _mailingAddressService;

        public UpdateMailingAddressUseCase(MailingAddressService mailingAddressService)
        {
            _mailingAddressService = mailingAddressService;
        }
        public async Task UpdateMailingAddress (int idAddress, MailingAddressDTO address, int userId)
        {
            var errors = new List<string>();

            if (!Validations.EsDireccionValida(address.Address))
                errors.Add("La dirección no es válida.");
            if (!Validations.EsDepartamentoValido(address.Department))
                errors.Add("El departamento no es válido.");
            if (!Validations.EsCiudadValida(address.City))
                errors.Add("La ciudad no es válida.");
            if (!Validations.EsBarrioValido(address.Neighborhood))
                errors.Add("El barrio no es válido.");
            if (!String.IsNullOrEmpty(address.Complement)){ 
                if (!Validations.EsComplementoValido(address.Complement)) 
                    errors.Add("El complemento no es válido.");
            }
            if (!Validations.NombreValido(address.ContactPersonName, address.ContactPersonLastName))
                errors.Add("El nombre o apellido del contacto no son válidos. Solo letras y espacios son permitidos.");
            if (!Validations.EsTelefonoValido(address.Phone))
                errors.Add("El teléfono no es válido. Debe contener solo dígitos y tener una longitud de 10 caracteres.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" | ", errors));

            var mailingAddress = MailingAddressDTO.FromDTOtoEntity(address, userId);

            await _mailingAddressService.UpdateMailingAddress(idAddress, mailingAddress, userId);
        }

    }
}
