using Microsoft.AspNetCore.Identity;

namespace WeddingWebsite.Data.Entities
{
    public class User : IdentityUser
    {
        public string? GroupName { get; set; }

        public string? DirectLoginCode { get; set; }

        public bool HasGuest { get; set; }

        public string? Name { get; set; }

        public string? GuestName { get; set; }

        public string? GuestEmail { get; set; }

        public string? SaveTheDateAnswer { get; set; }

        public string? SaveTheDateGuestAnswer { get; set; }
    }
}
