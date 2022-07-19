using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Controllers
{
    public class DirectLoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public DirectLoginController(ApplicationDbContext db, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _db = db;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [Route("dr")]
        public async Task<IActionResult> Index(string code, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var user = await _db.Users.FirstOrDefaultAsync(e => e.DirectLoginCode == code);
            if (user == null)
            {
                return Unauthorized();
            }

            await _signInManager.SignInAsync(user, true);
            return LocalRedirect(returnUrl);
        }

        [Route("bad")]
        public IActionResult Bad()
        {
            return Ok(_configuration.GetConnectionString("DefaultConnection"));
            //return View();
        }

        [Route("env")]
        public IActionResult Env()
        {
            return Ok(Environment.GetEnvironmentVariable("CONNECTIONSTRINGS_DEFAULTCONNECTION") ?? "not found");
            //return View();
        }
    }
}
