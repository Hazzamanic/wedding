using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Pages
{
    [Authorize(Roles = "Admin")]
    public class ListUsersModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public ListUsersModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<User> Users { get; set; } = new List<User>();

        public async Task OnGet()
        {
            var users = await _db.Users.ToListAsync();

            var userRoles = (await _db.UserRoles.ToListAsync()).Select(e => e.UserId);

            Users = users.Where(e => !userRoles.Contains(e.Id)).ToList();
        }
    }
}
