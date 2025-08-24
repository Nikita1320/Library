using System.ComponentModel.DataAnnotations;

namespace Library.OrderMicroservice.Domain.Models
{
    public enum OrderStatus
    {
        Pending,
        Completed,
        Rejected
    }
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
