using Domain.Entities.Business;

namespace Domain.Entities.Users
{
    public class InvitationCodeEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public int BusinessId { get; set; }
        public BusinessEntity Business { get; set; } = null;
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; } = null;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
