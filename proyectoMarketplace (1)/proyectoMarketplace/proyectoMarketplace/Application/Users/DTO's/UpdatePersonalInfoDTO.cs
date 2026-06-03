using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTO_s
{
    public class UpdatePersonalInfoDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public required string Names { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public required string LastNames { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public required string Phone { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        public static UpdatePersonalInfoDTO FromEntityToDTO(UserEntity entity) 
        { 
            return new UpdatePersonalInfoDTO
            {
                Names = entity.Name,
                LastNames = entity.LastName,
                Phone = entity.Phone
            };
        }
            
    }
}
