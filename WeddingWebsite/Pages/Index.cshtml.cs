using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WeddingWebsite.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            RedirectToPage(nameof(SaveTheDateModel));
        }
    }
}
