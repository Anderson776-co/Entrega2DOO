using Application.Orders.DTOs;
using Application.Orders.UseCase;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIDazma.Controllers.Orders
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly RegisterOrderUseCase _registerOrderUseCase;
        private readonly GetOrderUseCase _getOrderUseCase;
        private readonly UpdateStateOrderUseCase _updateStateOrderUseCase;
        private readonly DeleteOrderByIdUseCase _deleteOrderByIdUseCase;

        public OrderController(RegisterOrderUseCase registerOrderUseCase, GetOrderUseCase getOrderUseCase, UpdateStateOrderUseCase updateStateOrderUseCase, DeleteOrderByIdUseCase deleteOrderByIdUseCase)
        {
            _registerOrderUseCase = registerOrderUseCase;
            _getOrderUseCase = getOrderUseCase;
            _updateStateOrderUseCase = updateStateOrderUseCase;
            _deleteOrderByIdUseCase = deleteOrderByIdUseCase;
        }

        [HttpPost("CreateOrder/{idPublication}/{quantity}/{mailingAddressId}")]
        public async Task<IActionResult> RegisterOrder(int idPublication, int quantity, int mailingAddressId)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)    
                throw new UnauthorizedException("El usuario no esta autenticado");

            var createdOrder = await _registerOrderUseCase.RegisterOrder(idPublication, int.Parse(authenticatedUserId), quantity, mailingAddressId);
            return Created("", new { message = "Orden creada", order = createdOrder });
        }

        [HttpGet("GetOrder")]
        public async Task<IActionResult> GetOrder()
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
                throw new UnauthorizedException("El usuario no esta autenticado");

            var orders = await _getOrderUseCase.GetOrdersByUserId(int.Parse(authenticatedUserId));
            return Ok(orders);
        }

        [HttpPatch("UpdateOrder/{state}/{idOrder}")]
        public async Task<IActionResult> UpdateStateOrder(string state, int idOrder)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
                throw new UnauthorizedException("El usuario no esta autenticado");

            var updatedOrder = await _updateStateOrderUseCase.UpdateStateOrder(int.Parse(authenticatedUserId), state, idOrder);
            return Ok(updatedOrder);
        }

        [HttpDelete("DeleteOrder/{idOrder}")]
        public async Task<IActionResult> DeleteOrder(int idOrder)
        {
            var authenticatedUserId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (authenticatedUserId == null)
                throw new UnauthorizedException("El usuario no esta autenticado");

            await _deleteOrderByIdUseCase.DeleteOrder(int.Parse(authenticatedUserId), idOrder);
            return Ok(new { message = "Orden eliminada" });
        }
    }
}
