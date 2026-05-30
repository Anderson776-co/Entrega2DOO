using Domain.Entities.Users;
using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.DTOs
{
    public class UserDTO
    {
        [Required(ErrorMessage = "El campo del nombre es obligatorio")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "El campo del apellido es obligatorio")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "El campo del teléfono es obligatorio")]
        public required string Phone { get; set; }
        [Required(ErrorMessage = "El campo del nombre de usuario es obligatorio")]
        public required string Username { get; set; }
        [Required(ErrorMessage = "El campo del correo electrónico es obligatorio")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "El campo de la contraseña es obligatorio")]
        public required string Password { get; set; }

        public static UserEntity FromDTOtoEntity(UserDTO userDTO) { 
            return new UserEntity
            {
                Name = string.Join(" ", userDTO.Name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                LastName = string.Join(" ", userDTO.LastName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)),
                Phone = userDTO.Phone.Replace(" ", ""),
                Username = userDTO.Username.Trim(),
                Email = userDTO.Email.Trim(),
                Password = userDTO.Password,
                RegisterType = RegisterType.Personal,
                BusinessId = null
            };
        }

        public static UserDTO FromEntityToDTO(UserEntity user)
        {
            return new UserDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            };
        }
    }
}
