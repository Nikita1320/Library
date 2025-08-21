using FluentValidation;
using FluentValidation.Results;
using Library.UserMicroservice.Api.DTOs;
using Library.UserMicroservice.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace Library.UserMicroservice.Api.Controllers
{
    public class UserController(UserService userService, IValidator<UserCreateDto> createUserValidator, IValidator<UserLoginDto> loginUserValidator) : Controller
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto dto)
        {
            ValidationResult validationResult = await createUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await userService.Register(dto.Login, dto.Email, dto.Username, dto.Password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            ValidationResult validationResult = await loginUserValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var token = await userService.Login(dto.UserLogin, dto.Password);
            return Ok(token);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
