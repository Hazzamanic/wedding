using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    [Authorize]
    public class TravelModel : PageModel
    {
        private readonly ILogger<TravelModel> _logger;
        private readonly UserManager<User> UserManager;
        private readonly ApplicationDbContext _db;

        public TravelModel(ILogger<TravelModel> logger, UserManager<User> userManager, ApplicationDbContext db)
        {
            _logger = logger;
            UserManager = userManager;
            _db = db;
        }

        public async Task OnGet(bool update = false)
        {
            var user = await UserManager.GetUserAsync(User);
        }
    }
}