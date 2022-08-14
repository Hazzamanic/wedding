using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WeddingWebsite.Pages
{
    public class AdminModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            if (User.IsInRole("Admin"))
            {
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
