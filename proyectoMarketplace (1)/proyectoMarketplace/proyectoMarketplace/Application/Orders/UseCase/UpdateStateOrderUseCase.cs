using Application.Orders.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services.Orders;

namespace Application.Orders.UseCase
{
    public class UpdateStateOrderUseCase
    {
        private readonly OrderService _orderService;

        public UpdateStateOrderUseCase(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<OrderDTO> UpdateStateOrder(int idUser, string newState, int idOrder)
        {
            var errors = new List<string>();
            if (idOrder <= 0)
                errors.Add("El ID de la orden no es válido");

            if (!Enum.TryParse<Status>(newState, ignoreCase: true, out Status statusEnum))
                errors.Add($"El estado '{newState}' no es válido. Los estados permitidos son: Pending, Confirmed, Cancelled.");

            if (errors.Count > 0)
                throw new ValidationException(string.Join(", ", errors));

            var updatedOrder = await _orderService.UpdateOrderStatus(idUser, statusEnum, idOrder);
            return OrderDTO.FromEntityToDTO(updatedOrder);
        }
    }
}
