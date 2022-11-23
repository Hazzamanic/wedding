using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    [Authorize]
    public class ConfirmationModel : PageModel
    {
        private readonly ILogger<ConfirmationModel> _logger;
        private readonly UserManager<User> UserManager;
        private readonly ApplicationDbContext _db;

        public ConfirmationModel(ILogger<ConfirmationModel> logger, UserManager<User> userManager, ApplicationDbContext db)
        {
            _logger = logger;
            UserManager = userManager;
            _db = db;
        }

        public class InputModel
        {
            public AnswerModel Guest1 { get; set; } = new AnswerModel();
            public AnswerModel Guest2 { get; set; } = new AnswerModel();
            public string MoreInfo { get; set; }
        }

        public class AnswerModel
        {
            public string Name { get; set; }
            public string IsAttending { get; set; }
            public string PizzaParty { get; set; }
            public string WeddingDay { get; set; }
            public string Brunch { get; set; }

            public string JoinAccommodationList { get; set; }
            public string DietaryRequirements { get; set; }
            public string SongRequest { get; set; }

        }

        public User CurrentUser { get; set; }

        public string? Name { get; set; }

        public bool CanSubmit { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task OnGet(bool update = false)
        {
            var user = await UserManager.GetUserAsync(User);

            CurrentUser = user;
            CanSubmit = update || string.IsNullOrWhiteSpace(user.SaveTheDateAnswer);

            Input = new InputModel();

            Input.Guest1.IsAttending = user.Guest1IsAttending;
            Input.Guest1.PizzaParty = user.Guest1PizzaParty;
            Input.Guest1.Brunch = user.Guest1Brunch;
            Input.Guest1.JoinAccommodationList = user.Guest1AccommodationList;
            Input.Guest1.DietaryRequirements = user.Guest1DietaryRequirements;
            Input.Guest1.SongRequest = user.Guest1SongRequest;
            Input.Guest1.Name = user.Name;

            Input.Guest2.IsAttending = user.Guest2IsAttending; 
            Input.Guest2.PizzaParty = user.Guest2PizzaParty; 
            Input.Guest2.Brunch = user.Guest2Brunch;
            Input.Guest2.JoinAccommodationList = user.Guest2AccommodationList;
            Input.Guest2.DietaryRequirements = user.Guest2DietaryRequirements;
            Input.Guest2.SongRequest = user.Guest2SongRequest;
            Input.Guest2.Name = user.GuestName;

            Input.MoreInfo = user.MoreInfoRequest;


            Name = !string.IsNullOrWhiteSpace(user.GroupName) ?
                user.GroupName :
                string.IsNullOrWhiteSpace(user.GuestName) ?
                    user.Name :
                    $"{user.Name} & {user.GuestName}";
        }
    }
}