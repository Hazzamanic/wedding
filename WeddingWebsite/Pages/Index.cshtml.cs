using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<User> UserManager;
        private readonly ApplicationDbContext _db;

        public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager, ApplicationDbContext db)
        {
            _logger = logger;
            UserManager = userManager;
            _db = db;
        }

        public class InputModel
        {
            public string? Answer { get; set; }
            public string? GuestAnswer { get; set; }
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

            Input = new InputModel
            {
                Answer = user.SaveTheDateAnswer,
                GuestAnswer = user.SaveTheDateGuestAnswer
            };

            Name = !string.IsNullOrWhiteSpace(user.GroupName) ?
                user.GroupName :
                string.IsNullOrWhiteSpace(user.GuestName) ?
                    user.Name :
                    $"{user.Name} & {user.GuestName}";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await UserManager.GetUserAsync(User);

            user.SaveTheDateAnswer = Input.Answer;
            user.SaveTheDateGuestAnswer = Input.GuestAnswer;

            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}