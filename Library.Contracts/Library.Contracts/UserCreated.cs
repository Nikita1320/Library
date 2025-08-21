namespace Library.Contracts
{
    public interface UserCreated
    {
        Guid UserId { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        DateTime CreatedTime { get; set; }
    }
}
