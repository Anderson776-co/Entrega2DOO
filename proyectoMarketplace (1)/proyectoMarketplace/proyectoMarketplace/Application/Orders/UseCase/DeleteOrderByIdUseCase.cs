using Domain.Exceptions;
using Domain.Services.Orders;

namespace Application.Orders.UseCase
{
    public class DeleteOrderByIdUseCase
    {
        private readonly OrderService _orderService;
        public DeleteOrderByIdUseCase(OrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task DeleteOrder(int idUser, int idOrder)
        {
            if (idOrder <= 0)
                throw new ValidationException("El ID de la orden no es válido");

            await _orderService.DeleteOrder(idUser, idOrder);
        }
    }
}
