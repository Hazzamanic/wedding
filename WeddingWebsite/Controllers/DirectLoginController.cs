using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Controllers
{
    public class DirectLoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<User> _signInManager;

        public DirectLoginController(ApplicationDbContext db, SignInManager<User> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
        }

        [Route("dr")]
        public async Task<IActionResult> Index(string code, string? redirectTo)
        {
            var user = _db.Users.FirstOrDefault(e => e.DirectLoginCode == code);
            if (user == null)
            {
                return Unauthorized();
            }

            await _signInManager.SignInAsync(user, true);
            return RedirectToRoute("~/");
        }
    }
}
