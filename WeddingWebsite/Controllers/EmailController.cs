using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeddingWebsite.Data.Entities;
using WeddingWebsite.Services;

namespace WeddingWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("email/{action}/{id?}")]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;

        public EmailController(IEmailService emailService, UserManager<User> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SendInvite(string[] ids, bool isTest)
        {
            string? testEmail = null;
            if (isTest)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                testEmail = currentUser.Email;
            }

            var results = await _emailService.SendSaveTheDate(ids, testEmail);

            return Json(results);
        }
    }
}