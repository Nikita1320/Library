namespace Library.UserMicroservice.Api.DTOs
{
    public class UserCreateDto
    {
        public string Login { get; set; } = string.Empty;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
