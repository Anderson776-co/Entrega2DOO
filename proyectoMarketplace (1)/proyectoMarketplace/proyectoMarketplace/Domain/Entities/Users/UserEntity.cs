using Domain.Models;
using Domain.Entities.Business;

namespace Domain.Entities.Users
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string LastName { get; set; } = "";
        public RegisterType RegisterType { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Password { get; set; } = "";
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; } = null!;
        public int? BusinessId { get; set; }
        public BusinessEntity? Business { get; set; } = null;
        public bool IsActive { get; set; }
        public ICollection<MailingAddressEntity> MailingAddresses { get; set; } = new List<MailingAddressEntity>();
    }
}
