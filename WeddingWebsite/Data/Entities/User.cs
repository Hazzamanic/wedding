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

        public bool HasSentSaveTheDateEmail { get; set; }

        public bool HasSentGuestSaveTheDateEmail { get; set; }
        public string Guest1IsAttending { get; set; }
        public string Guest1PizzaParty { get; set; }
        public string Guest1Brunch { get; set; }
        public string Guest1DietaryRequirements { get; set; }
        public string Guest1SongRequest { get; set; }
        public string Guest1AccommodationList { get; set; }
        public string Guest2IsAttending { get; set; }
        public string Guest2PizzaParty { get; set; }
        public string Guest2Brunch { get; set; }
        public string Guest2DietaryRequirements { get; set; }
        public string Guest2SongRequest { get; set; }
        public string Guest2AccommodationList { get; set; }
        public string MoreInfoRequest { get; set; }
        public string PartyType { get; set; }
        public bool HasResponded { get; set; }
    }
}
