// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    [Authorize(Roles = "Admin")]
    public class CreateUserModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<EditUserModel> _logger;
        private readonly IEmailSender _emailSender;

        public CreateUserModel(
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

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string GuestName { get; set; }
            public string GuestEmail { get; set; }
            public string GroupName { get; set; }
        }


        public async Task OnGetAsync(string id)
        {
            Input = new InputModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new User();

                user.DirectLoginCode = Guid.NewGuid().ToString();
                user.Name = Input.Name;
                user.GroupName = Input.GroupName;
                user.GuestName = Input.GuestName;
                user.GuestEmail = Input.GuestEmail;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Guid.NewGuid().ToString());

                if(result.Succeeded)
                {
                    return RedirectToPage("EditUser", new { id = user.Id });
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
