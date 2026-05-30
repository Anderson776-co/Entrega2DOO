using Application.Orders.DTOs;
using Domain.Services.Orders;

namespace Application.Orders.UseCase
{
    public class GetOrderUseCase
    {
        private readonly OrderService _orderService;

        public GetOrderUseCase(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<List<OrderDTO>> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);
            return orders.Select(OrderDTO.FromEntityToDTO).ToList();
        }
    }
}
