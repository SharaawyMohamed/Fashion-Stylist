namespace Web.Domain.DTOs.ProductDTO
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public double Rate { get; set; }
        public DateOnly Date { get; set; }
        public UserDTO User { get; set; }
    }
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
