using FluentValidation;
using Library.OrderMicroservice.Api.DTOs;
using Library.OrderMicroservice.Api.Validators;
using Library.OrderMicroservice.Application.Services;
using Library.OrderMicroservice.Domain.Interfaces;
using Library.OrderMicroservice.Infrastructure.Contexts;
using Library.OrderMicroservice.Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.OrderMicroservice.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("UserMicroserviceDbConnectionString"));
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });
            builder.Services.AddScoped<IValidator<OrderDto>, OrderDtoValidator>();
            builder.Services.AddScoped<IValidator<OrderItemDto>, OrderItemDtoValidator>();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<OrderService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
