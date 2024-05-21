using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    public class FaqModel : PageModel
    {
        private readonly ILogger<FaqModel> _logger;
        private readonly UserManager<User> UserManager;
        private readonly ApplicationDbContext _db;

        public FaqModel(ILogger<FaqModel> logger, UserManager<User> userManager, ApplicationDbContext db)
        {
            _logger = logger;
            UserManager = userManager;
            _db = db;
        }

        public async Task OnGet()
        {
        }
    }
}