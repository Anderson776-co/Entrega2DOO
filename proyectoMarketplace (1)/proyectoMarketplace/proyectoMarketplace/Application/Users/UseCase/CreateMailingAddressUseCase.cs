using Application;
using Application.Users.DTOs;
using Domain.Entities.Users;
using Domain.Services.Users;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.UseCase
{
    public class CreateMailingAddressUseCase
    {
        private readonly MailingAddressService _mailingAddressService;

        public CreateMailingAddressUseCase(MailingAddressService mailingAddressService)
        {
            _mailingAddressService = mailingAddressService;
        }
        public async Task<MailingAddressDTO> CreateMailingAddress(MailingAddressDTO address, int idUser)
        {
            var errors = new List<string>();
            
            if (!Validations.EsDireccionValida(address.Address))
                errors.Add("La dirección no es válida. Use un formato colombiano estándar, por ejemplo: 'Calle 123 #45-67'");
            if (!Validations.EsDepartamentoValido(address.Department))
                errors.Add("El departamento no es válido. Solo letras y espacios son permitidos.");
            if (!Validations.EsCiudadValida(address.City))
                errors.Add("La ciudad no es válida. Solo letras y espacios son permitidos.");
            if (!Validations.EsBarrioValido(address.Neighborhood))
                errors.Add("El barrio no es válido. Solo letras, números y espacios son permitidos.");
            if (!Validations.EsTelefonoValido(address.Phone))
                errors.Add("El teléfono no es válido. Debe contener solo dígitos y tener una longitud de 10 caracteres.");
            if (!Validations.NombreValido(address.ContactPersonName, address.ContactPersonLastName))
                errors.Add("El nombre o apellido del contacto no son válidos. Solo letras y espacios son permitidos.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" | ", errors));

            MailingAddressEntity mailingAddressEntity = MailingAddressDTO.FromDTOtoEntity(address, idUser);
            MailingAddressEntity createdAddress = await _mailingAddressService.CreateAddress(mailingAddressEntity);
            return MailingAddressDTO.FromEntityToDTO(createdAddress);
        }
    }   
}
