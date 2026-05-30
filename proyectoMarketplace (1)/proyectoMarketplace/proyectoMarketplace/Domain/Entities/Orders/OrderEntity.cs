using Domain.Models;

namespace Domain.Entities.Order
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PublicationId { get; set; }
        public int MailingAddressId { get; set; }
        public int Quantity { get; set; } 
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Status Status { get; set; }
    }
}
