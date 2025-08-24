using Library.OrderMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Library.OrderMicroservice.Infrastructure.Contexts
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options){ }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems {  get; set; }
    }
}
