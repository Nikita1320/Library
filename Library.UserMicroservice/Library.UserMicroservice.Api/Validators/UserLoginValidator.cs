using FluentValidation;
using Library.UserMicroservice.Api.DTOs;

namespace Library.UserMicroservice.Api.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.UserLogin).NotEmpty();
            RuleFor(u => u.Password).NotEmpty();
        }
    }
}
