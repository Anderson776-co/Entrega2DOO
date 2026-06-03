using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTO_s
{
    public class UserInfoDTO
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    

    public static UserInfoDTO FromEntityToDTO(UserEntity entity)
        {
            return new UserInfoDTO
            {
                Name = entity.Name,
                LastName = entity.LastName,
                Phone = entity.Phone,
                Email = entity.Email
            };
        }
    }
}
