using Library.Contracts;
using Library.OrderMicroservice.Domain.Interfaces;
using Library.OrderMicroservice.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.OrderMicroservice.Application.Services
{
    public class OrderService(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint, ILogger<OrderService> logger)
    {
        public async Task<Guid> CreateOrderAsync(Guid userId, Dictionary<Guid, int> items)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            order.Items = items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                BookId = i.Key,
                Quantity = i.Value,
                OrderId = order.Id // will be set by EF automatically
            }).ToList();

            await orderRepository.AddOrderAsync(order);

            logger.LogInformation("Order {OrderId} created with status Pending", order.Id);

            var bookQuantities = order.Items.Select(i => new BookQuantity(i.BookId, i.Quantity)).ToList();

            await publishEndpoint.Publish<StockCheckRequest>(new
            {
                OrderId = order.Id,
                Items = bookQuantities
            });

            logger.LogInformation("Published StockCheckRequest for Order {OrderId}", order.Id);

            return order.Id;
        }

        public Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return orderRepository.GetOrderByIdAsync(orderId);
        }
    }
}
