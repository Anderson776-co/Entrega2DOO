using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.DTOs
{
    public class MailingAddressDTO
    {
        [Required(ErrorMessage = "El campo de la dirección es obligatorio")]
        public required string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del departamento es obligatorio")]
        public required string Department { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de la ciudad es obligatorio")]
        public required string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del barrio es obligatorio")]
        public required string Neighborhood { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del complemento es obligatorio")]
        public required string Complement { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del nombre de la persona de contacto es obligatorio")]
        public required string ContactPersonName { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del apellido de la persona de contacto es obligatorio")]
        public required string ContactPersonLastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo del teléfono es obligatorio")]
        public required string Phone { get; set; } = string.Empty;

        public static MailingAddressEntity FromDTOtoEntity(MailingAddressDTO mailingAddressDTO, int idUser)
        {
            return new MailingAddressEntity
            {
                Address = mailingAddressDTO.Address.Trim(),
                Department = string.Join(" ",mailingAddressDTO.Department.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                City = string.Join(" ",mailingAddressDTO.City.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                Neighborhood = string.Join(" ",mailingAddressDTO.Neighborhood.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                Complement = string.Join(" ",mailingAddressDTO.Complement.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                ContactPersonName = string.Join(" ",mailingAddressDTO.ContactPersonName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                ContactPersonLastName = string.Join(" ",mailingAddressDTO.ContactPersonLastName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                Phone = mailingAddressDTO.Phone.Replace(" ", ""),
                UserId = idUser
            };
        }

        public static MailingAddressDTO FromEntityToDTO(MailingAddressEntity entity)
        {
            return new MailingAddressDTO
            {
                Address = entity.Address,
                Department = entity.Department,
                City = entity.City,
                Neighborhood = entity.Neighborhood,
                Complement = entity.Complement,
                ContactPersonName = entity.ContactPersonName,
                ContactPersonLastName = entity.ContactPersonLastName,
                Phone = entity.Phone
            };
        }
    }
}
