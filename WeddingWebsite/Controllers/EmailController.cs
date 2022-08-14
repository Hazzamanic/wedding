using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingWebsite.Services;

namespace WeddingWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("email/{action}/{id?}")]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendInvite(string[] ids)
        {
            await _emailService.SendInvite(ids);

            return Redirect("/admin");
        }
    }
}
