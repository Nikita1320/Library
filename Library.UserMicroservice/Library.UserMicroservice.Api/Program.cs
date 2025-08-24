using FluentValidation;
using Library.UserMicroservice.Api.DTOs;
using Library.UserMicroservice.Api.Extensions;
using Library.UserMicroservice.Api.Validators;
using Library.UserMicroservice.Application.Services;
using Library.UserMicroservice.Application.Settings;
using Library.UserMicroservice.Domain.Interfaces;
using Library.UserMicroservice.Infrastructure.Contexts;
using Library.UserMicroservice.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Library.UserMicroservice.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<UserDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("UserMicroserviceDbConnectionStringDbConnectionString"));
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                       new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             }
                         },
                        new string[] {}
                     }
                });
                option.UseAllOfToExtendReferenceSchemas();
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Some API v1", Version = "v1" });
            });

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
            builder.Services.AddScoped<IValidator<UserCreateDto>, UserCreateValidator>();
            builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginValidator>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<UserService>();

            builder.Services.Configure<AuthSettings>(
                builder.Configuration.GetSection(key: "AuthSettings"));
            builder.Services.AddAuth(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTAuthDemo v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
