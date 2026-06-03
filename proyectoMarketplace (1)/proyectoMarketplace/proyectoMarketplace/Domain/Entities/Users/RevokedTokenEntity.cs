
namespace Domain.Entities.Users
{
    public class RevokedTokenEntity
    {
        public int Id { get; set; }
        public string Jti { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime RevokedAt { get; set; }
    }
}
