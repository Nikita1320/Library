using FluentValidation;
using FluentValidation.Results;
using Library.OrderMicroservice.Api.DTOs;
using Library.OrderMicroservice.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.OrderMicroservice.Api.Controllers
{
    public class OrderController(OrderService orderService, IValidator<OrderDto> validator,ILogger<OrderController> logger) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            ValidationResult validationResult = await validator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var userId = User.FindFirstValue("Id") ?? throw new Exception("Error Authentication");
            var orderId = await orderService.CreateOrderAsync(new Guid(userId), orderDto.Items.ToDictionary(i => i.BookId, i => i.Quantity));

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
