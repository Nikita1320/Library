using FluentValidation;
using FluentValidation.Results;
using Library.OrderMicroservice.Api.Consumers;
using Library.OrderMicroservice.Api.DTOs;
using Library.OrderMicroservice.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.OrderMicroservice.Api.Controllers
{
    public class OrderController(OrderService orderService, IValidator<OrderDto> validator,ILogger<OrderController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            ValidationResult validationResult = await validator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var orderId = await orderService.CreateOrderAsync(orderDto);

            logger.LogInformation("Order {OrderId} creation requested", orderId);

            return CreatedAtAction(nameof(GetOrder), new { orderId }, new { orderId });
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var order = await orderService.GetOrderByIdAsync(orderId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}
