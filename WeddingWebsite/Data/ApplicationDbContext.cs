using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Data.Entities;
using WeddingWebsite.Models;

namespace WeddingWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WeddingWebsite.Models.UserEditViewModel>? UserEditViewModel { get; set; }
    }
}