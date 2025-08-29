using Microsoft.AspNetCore.Identity;

namespace Web.Domain.Entites
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string FCM_Token { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; } = string.Empty;
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
        public ICollection<Transactions> transactions { get; set; } = new HashSet<Transactions>();
    }
}
