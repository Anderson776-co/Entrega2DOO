using Domain.Entities.Order;
using Domain.Ports.Order;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _oContext;
        public OrderRepository(OrderDbContext oContext)
        {
            _oContext = oContext;
        }

        public async Task DeleteOrder(OrderEntity order)
        {
            _oContext.Orders.Remove(order);
            await _oContext.SaveChangesAsync();
        }

        public async Task<OrderEntity?> GetOrderById(int idOrder, int idUser)
        {
            return await _oContext.Orders.FirstOrDefaultAsync(o => o.Id == idOrder && o.UserId == idUser);
        }

        public async Task<List<OrderEntity>> GetOrdersByUserId(int userId)
        {
            return await _oContext.Orders.Where(o => o.UserId == userId).ToListAsync();
        }
        public async Task<OrderEntity> RegisterOrder(OrderEntity order)
        {
            _oContext.Orders.Add(order);
            await _oContext.SaveChangesAsync();
            return order;
        }

        public async Task<OrderEntity> UpdateOrder(OrderEntity order)
        {
            _oContext.Orders.Update(order);
            await _oContext.SaveChangesAsync();
            return order;
        }
    }
}
