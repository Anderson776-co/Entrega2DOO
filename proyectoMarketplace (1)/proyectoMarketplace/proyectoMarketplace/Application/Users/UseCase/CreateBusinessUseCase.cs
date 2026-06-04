using Application.Users.DTOs;
using Application.Users.Interfaces;
using Domain.Entities.Business;
using Domain.Services.Users;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.UseCase
{
    public class CreateBusinessUseCase
    {
        private readonly BusinessService _businessService;
        private readonly UserService _userService;
        private readonly IjwtService _jwtService;

        public CreateBusinessUseCase(BusinessService businessService, UserService userService, IjwtService jwtService)
        {
            _businessService = businessService;
            _userService = userService;
            _jwtService = jwtService;
        }

        public async Task<(BusinessDTO business, string token)> CreateBusiness(BusinessDTO business, int adminUserId)
        {
            var errors = new List<string>();

            if (!Validations.IsValidBusinessName(business.Name))
                errors.Add("El nombre del negocio no es válido. Use un formato adecuado, por ejemplo: 'Empresa S.A.S', 'Distribuidora El Sol', 'Tech & Solutions Ltda'. Solo se permiten letras, números, espacios y los caracteres especiales . , - & ' ( )");
            if (!Validations.EsDireccionValida(business.Address))
                errors.Add("La dirección no es válida. Use un formato colombiano estándar, por ejemplo: 'Calle 123 #45-67'");
            if (!Validations.EsTelefonoValido(business.Phone))
                errors.Add("El teléfono no es válido. Debe contener solo dígitos y tener una longitud de 10 caracteres.");
            if (!Validations.EsCorreo(business.Email))
                errors.Add("Por favor, ingresa un correo electrónico válido. Ejemplo: nombre@dominio.com");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(" | ", errors));

            BusinessEntity businessEntity = BusinessDTO.FromDTOtoEntity(business, adminUserId);
            BusinessEntity createdBusiness = await _businessService.CreateBusiness(businessEntity, adminUserId);

            var updatedUser = await _userService.GetUserById(adminUserId);
            var newToken = _jwtService.GenerateToken(updatedUser.Email, updatedUser.Role.Name, updatedUser.Id);

            return (BusinessDTO.FromEntityToDTO(createdBusiness), newToken);
        }
    }
}