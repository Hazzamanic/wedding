// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    [Authorize(Roles = "Admin")]
    public class UploadModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<EditUserModel> _logger;
        private readonly IEmailSender _emailSender;

        public UploadModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<EditUserModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }


        [BindProperty]
        public IFormFile Upload { get; set; }


        public async Task OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using var stream = Upload.OpenReadStream();
            using var reader = new StreamReader(stream);
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var user = new User();

                    user.DirectLoginCode = Guid.NewGuid().ToString();
                    user.Name = csv.GetField(0);
                    user.GuestName = csv.GetField(1);
                    user.GroupName = csv.GetField(2);
                    var email = csv.GetField(3);
                    user.GuestEmail = csv.GetField(4);

                    var username = string.IsNullOrWhiteSpace(email)
                        ? Guid.NewGuid().ToString()
                        : email;

                    await _userStore.SetUserNameAsync(user, username, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, Guid.NewGuid().ToString());;
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
