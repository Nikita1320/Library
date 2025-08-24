using FluentValidation;
using Library.OrderMicroservice.Api.DTOs;

namespace Library.OrderMicroservice.Api.Validators
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("Order must contain at least one item.");
            RuleForEach(x => x.Items).SetValidator(new OrderItemDtoValidator());
        }
    }
}
