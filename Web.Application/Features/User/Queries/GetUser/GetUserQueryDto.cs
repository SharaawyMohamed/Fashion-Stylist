namespace Web.Application.Features.User.Queries.GetUser
{
    public class GetUserQueryDto
    {
        public string FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public string FCM_Token { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }
}
