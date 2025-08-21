namespace Library.Contracts
{
    public interface OrderCreated
    {
        Guid OrderId { get; set; }
        string Name { get; set; }
    }
}
