using WeddingWebsite.Constants;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Extensions
{
    public static class UserExtensions
    {
        public static string GetGroupName(this User user, string separator = "&")
        {
            if (!string.IsNullOrWhiteSpace(user.GroupName))
            {
                return user.GroupName;
            }

            if (string.IsNullOrWhiteSpace(user.GuestName))
            {
                return user.Name;
            }

            return $"{user.Name} {separator} {user.GuestName}";
        } 

        public static string GetAccommodationName(this User user)
        {
            switch(user.PartyType)
            {
                case PartyTypeConstants.Bridal:
                    return "The Apartments";
                default:
                    return "The Villas";
            }
        }
    }
}
