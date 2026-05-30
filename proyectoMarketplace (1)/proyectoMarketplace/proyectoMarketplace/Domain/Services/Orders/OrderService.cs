using Domain.Entities.Order;
using Domain.Exceptions;
using Domain.Models;
using Domain.Ports;
using Domain.Ports.Order;

namespace Domain.Services.Orders
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublicationRepository _publicationRepository;
        private readonly IMailingAddressRepository _mailingAddressRepository;

        public OrderService(IOrderRepository orderRepository, IPublicationRepository publicationRepository, IMailingAddressRepository mailingAddressRepository)
        {
            _orderRepository = orderRepository;
            _publicationRepository = publicationRepository;
            _mailingAddressRepository = mailingAddressRepository;
        }

        public async Task<OrderEntity> RegisterOrder(int idPublication, int idUser, int quantity, int mailingAddressId)
        {
            if (!await _publicationRepository.ExistsPublicationById(idPublication))
                throw new NonFoundException("Publicacion no encontrada");
            if (!await _mailingAddressRepository.AddressExistsById(idUser, mailingAddressId))
                throw new NonFoundException("Dirección de envío no encontrada");
            if (!await _publicationRepository.IsPublicationAvailable(idPublication))
                throw new ValidationException("La publicacion no está disponible");

            var publicationPrice = await _publicationRepository.GetPriceById(idPublication);

            var orderEntity = new OrderEntity
            {
                PublicationId = idPublication,
                UserId = idUser,
                Quantity = quantity,
                TotalAmount = publicationPrice * quantity,
                Status = Status.Pending,
                MailingAddressId = mailingAddressId
            };

            return await _orderRepository.RegisterOrder(orderEntity);
        }

        public async Task<List<OrderEntity>> GetOrdersByUserId(int userId)
        {
            return await _orderRepository.GetOrdersByUserId(userId);
        }

        public async Task<OrderEntity> UpdateOrderStatus(int idUser, Status newState, int idOrder)
        {
            var order = await _orderRepository.GetOrderById(idOrder, idUser);

            if (order == null)  
                throw new NonFoundException("Orden no encontrada");

            var publication = await _publicationRepository.GetById(order.PublicationId);
            if (publication == null)
                throw new NonFoundException("Publicación asociada a la orden no encontrada");
            
            if (publication.UserId != idUser)
                throw new UnauthorizedException("No tienes permiso para actualizar este pedido");

            order.Status = newState;
            return await _orderRepository.UpdateOrder(order);
        }

        public async Task DeleteOrder(int idUser, int idOrder)
        {
            var order = await _orderRepository.GetOrderById(idOrder, idUser);
            if (order == null)
                throw new NonFoundException("Orden no encontrada");
            order.Status = Status.Cancelled;
            await _orderRepository.UpdateOrder(order);
        }
    }
}
