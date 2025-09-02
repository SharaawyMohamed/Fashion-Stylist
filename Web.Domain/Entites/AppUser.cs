using Microsoft.AspNetCore.Identity;

namespace Web.Domain.Entites
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string FCM_Token { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; } = "https://res.cloudinary.com/doz1vpcgr/image/upload/v1756802120/ian-dooley-d1UPkiFd04A-unsplash_qy7eue.jpg";
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
        public ICollection<Transactions> transactions { get; set; } = new HashSet<Transactions>();
    }
}
