namespace Library.OrderMicroservice.Api.DTOs
{
    public record OrderDto(Guid UserId, List<OrderItemDto> Items);
}
