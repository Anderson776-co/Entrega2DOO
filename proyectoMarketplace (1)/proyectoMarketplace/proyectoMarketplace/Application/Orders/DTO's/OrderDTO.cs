using Domain.Entities.Order;
using Domain.Models;

namespace Application.Orders.DTOs
{
    public class OrderDTO
    {
        public int UserId { get; set; }
        public int PublicationId { get; set; }
        public int Quantity { get; set; }
        public int AddressId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "";

        public static OrderDTO FromEntityToDTO(OrderEntity orderEntity)
        {
            return new OrderDTO
            {
                UserId = orderEntity.UserId,
                PublicationId = orderEntity.PublicationId,
                Quantity = orderEntity.Quantity,
                AddressId = orderEntity.MailingAddressId,
                TotalAmount = orderEntity.TotalAmount,
                CreatedDate = orderEntity.CreatedDate,
                Status = orderEntity.Status.ToString()
            };
        }
    }
}
