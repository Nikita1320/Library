using FluentValidation;
using Library.OrderMicroservice.Api.DTOs;

namespace Library.OrderMicroservice.Api.Validators
{
    public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
    {
        public OrderItemDtoValidator()
        {
            RuleFor(x => x.BookId).NotEmpty().WithMessage("BookId must be provided.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
