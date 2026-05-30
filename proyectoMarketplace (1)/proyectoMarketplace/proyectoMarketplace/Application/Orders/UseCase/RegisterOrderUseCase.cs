using Application.Orders.DTOs;
using Domain.Exceptions;
using Domain.Services.Orders;

namespace Application.Orders.UseCase
{
    public class RegisterOrderUseCase
    {
        private readonly OrderService _orderService;

        public RegisterOrderUseCase(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<OrderDTO> RegisterOrder(int idPost, int idUser, int quantity, int mailingAddressId)
        {
            var errors = new List<string>();
            if (idPost <= 0)
                errors.Add("El ID del post no es válido");
            if (quantity <= 0)
                errors.Add("La cantidad no es válida");
            if (mailingAddressId <= 0)
                errors.Add("El ID de la dirección de envío no es válida");
            if (errors.Count > 0)
                throw new ValidationException(string.Join(", ", errors));

            var order = await _orderService.RegisterOrder(idPost, idUser, quantity, mailingAddressId);
            return OrderDTO.FromEntityToDTO(order);
        }
    }
}
