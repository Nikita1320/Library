using FluentValidation;
using Library.UserMicroservice.Api.DTOs;

namespace Library.UserMicroservice.Api.Validators
{
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateValidator()
        {
            RuleFor(u => u.Username).NotEmpty().Length(3, 50);
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6);
            RuleFor(u => u.Login).NotEmpty().MinimumLength(6);
        }
    }
}
