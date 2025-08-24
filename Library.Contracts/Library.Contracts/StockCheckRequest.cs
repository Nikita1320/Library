namespace Library.Contracts
{
    public interface StockCheckRequest
    {
        public Guid OrderId {  get; set; }
        public List<BookQuantity> Items { get; set; }
    }
}
