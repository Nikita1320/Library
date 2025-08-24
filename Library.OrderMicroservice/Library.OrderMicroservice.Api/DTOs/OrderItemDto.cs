namespace Library.OrderMicroservice.Api.DTOs
{
    public record OrderItemDto(Guid BookId, int Quantity);
}
