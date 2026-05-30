using Domain.Entities;
using Domain.Entities.Business;


namespace Application.Users.DTOs
{
    public class BusinessDTO
    {
        public string Name { get; set; } = "";
        public string NIT { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";

        public static BusinessEntity FromDTOtoEntity(BusinessDTO businessDTO, int adminUserId)
        {
            return new BusinessEntity {
                Name = businessDTO.Name,
                NIT = businessDTO.NIT,
                Address = businessDTO.Address,
                Phone = businessDTO.Phone,
                Email = businessDTO.Email,
                AdminUserId = adminUserId
            };

        }

        public static BusinessDTO FromEntityToDTO(BusinessEntity businessEntity)
        {
            return new BusinessDTO
            {
                Name = businessEntity.Name,
                NIT = businessEntity.NIT,
                Address = businessEntity.Address,
                Phone = businessEntity.Phone,
                Email = businessEntity.Email
            };
        }
    }
}
