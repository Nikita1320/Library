namespace Library.Contracts
{
    public interface StockResponce
    {
        public Guid OrderId { get; set; }
        public bool IsAvailable { get; set; }
        public List<Guid>? UnavailableProductIds {  get; set; }
    }
}
