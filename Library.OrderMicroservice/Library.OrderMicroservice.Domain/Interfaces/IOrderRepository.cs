using Library.OrderMicroservice.Domain.Models;

namespace Library.OrderMicroservice.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task UpdateOrderAsync(Order order);
    }
}
