using Library.Contracts;
using Library.OrderMicroservice.Domain.Interfaces;
using Library.OrderMicroservice.Domain.Models;
using MassTransit;

namespace Library.OrderMicroservice.Api.Consumers
{
    public class StockResponseConsumer(IOrderRepository orderRepository, ILogger<StockResponseConsumer> logger) : IConsumer<StockResponce>
    {
        public async Task Consume(ConsumeContext<StockResponce> context)
        {
            var message = context.Message;
            var order = await orderRepository.GetOrderByIdAsync(message.OrderId);

            if (order == null)
            {
                logger.LogWarning("Order {OrderId} not found for stock response", message.OrderId);
                return;
            }

            order.Status = message.IsAvailable ? OrderStatus.Completed : OrderStatus.Rejected;

            await orderRepository.UpdateOrderAsync(order);

            logger.LogInformation("Order {OrderId} updated status to {Status}", order.Id, order.Status);
        }
    }
}
