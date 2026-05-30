using Domain.Entities.Order;

namespace Domain.Ports.Order
{
    public interface IOrderRepository
    {
        Task<OrderEntity> RegisterOrder(OrderEntity order);
        Task<List<OrderEntity>> GetOrdersByUserId(int userId);
        Task<OrderEntity?> GetOrderById(int idOrder, int idUser);
        Task<OrderEntity> UpdateOrder(OrderEntity order);
        Task DeleteOrder(OrderEntity order);
    }
}
