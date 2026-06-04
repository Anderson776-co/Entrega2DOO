using Application.Users.DTOs;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DTO_s.ResponseDTO
{
    public class AddressResponseDTO
    {
        public int Id { get; set; }
        public string Address { get; set; } = "";
        public string Department { get; set; } = "";
        public string City { get; set; } = "";
        public string Neighborhood { get; set; } = "";
        public string Complement { get; set; } = "";
        public string ContactPersonName { get; set; } = "";
        public string ContactPersonLastName { get; set; } = "";
        public string Phone { get; set; } = "";

        public static AddressResponseDTO FromEntityToDTO(MailingAddressEntity entity)
        {
            return new AddressResponseDTO
            {
                Id = entity.Id,
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
