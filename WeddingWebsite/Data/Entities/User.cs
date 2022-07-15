using Microsoft.AspNetCore.Identity;

namespace WeddingWebsite.Data.Entities
{
    public class User : IdentityUser
    {
        public string? GroupName { get; set; }

        public string DirectLoginCode { get; set; }
    }
}
