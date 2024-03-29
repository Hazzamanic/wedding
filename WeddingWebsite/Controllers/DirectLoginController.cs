﻿using Microsoft.AspNetCore.Authentication;
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
        public async Task<IActionResult> Index(string code, string? p = null)
        {
            var redirect = "/Home";

            if (p == "f")
            {
                redirect = "/FAQ";
            }
            else if (p == "a")
            {
                redirect = "/Accommodation";
            }

            var user = await _db.Users.FirstOrDefaultAsync(e => e.DirectLoginCode == code);
            if (user == null)
            {
                return Unauthorized();
            }

            await _signInManager.SignInAsync(user, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1),

            });

            return RedirectToPage(redirect);
        }
    }
}
