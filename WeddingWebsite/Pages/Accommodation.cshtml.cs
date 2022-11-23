using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    [Authorize]
    public class AccommodationModel : PageModel
    {
        private readonly ILogger<AccommodationModel> _logger;
        private readonly UserManager<User> UserManager;
        private readonly ApplicationDbContext _db;

        public AccommodationModel(ILogger<AccommodationModel> logger, UserManager<User> userManager, ApplicationDbContext db)
        {
            _logger = logger;
            UserManager = userManager;
            _db = db;
        }

        public User CurrentUser { get; set; }

        public async Task OnGet()
        {
            var user = await UserManager.GetUserAsync(User);

            CurrentUser = user;
        }
    }
}