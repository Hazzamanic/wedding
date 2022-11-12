using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    [Authorize]
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly UserManager<User> UserManager;
        private readonly ApplicationDbContext _db;

        public RegisterModel(ILogger<RegisterModel> logger, UserManager<User> userManager, ApplicationDbContext db)
        {
            _logger = logger;
            UserManager = userManager;
            _db = db;
        }

        public class InputModel
        {
            public AnswerModel Guest1 { get; set; } = new AnswerModel();
            public AnswerModel Guest2 { get; set; } = new AnswerModel();
        }

        public class AnswerModel
        {
            
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

            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}