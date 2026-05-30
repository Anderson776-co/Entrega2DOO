using Application.Users.DTOs;
using Domain.Entities.Business;
using Domain.Services.Users;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.UseCase
{
    public class CreateBusinessUseCase
    {
        private readonly BusinessService _businessService;

        public CreateBusinessUseCase(BusinessService businessService)
        {
            _businessService = businessService;
        }

        public async Task<BusinessDTO> CreateBusiness(BusinessDTO business, int adminUserId)
        {
            var errors = new List<string>();

            if (!Validations.IsValidBusinessName(business.Name))
                errors.Add("El nombre del negocio no es válido. Use un formato adecuado, por ejemplo: 'Empresa S.A.S', 'Distribuidora El Sol', 'Tech & Solutions Ltda'. Solo se permiten letras, números, espacios y los caracteres especiales . , - & ' ( )");
            if (!Validations.EsDireccionValida(business.Address))
                errors.Add("La dirección no es válida. Use un formato colombiano estándar, por ejemplo: 'Calle 123 #45-67'");
            if (!Validations.EsTelefonoValido(business.Phone))
                errors.Add("El teléfono no es válido. Debe contener solo dígitos y tener una longitud de 10 caracteres.");
            if (!Validations.EsCorreo(business.Email))
                errors.Add("El correo electrónico no es válido. Use un formato adecuado, por ejemplo: 'empresa@dominio.com', solo se permiten letras, números, puntos, guiones bajos y guiones antes del @.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" | ", errors));

            BusinessEntity businessEntity = BusinessDTO.FromDTOtoEntity(business, adminUserId);
            BusinessEntity createdBusiness = await _businessService.CreateBusiness(businessEntity);
            return BusinessDTO.FromEntityToDTO(createdBusiness);
        }
    }
}