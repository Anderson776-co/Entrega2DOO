using Domain.Entities.Users;

namespace Domain.Entities.Business
{
    public class BusinessEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string NIT { get; set; } = "";

        public string Address { get; set; } = "";

        public string Phone { get; set; } = "";

        public string Email { get; set; } = "";

        public int AdminUserId { get; set; }

        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}