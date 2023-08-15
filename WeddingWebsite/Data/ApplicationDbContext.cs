using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public DbSet<Gift> Gifts { get; set; }

        public DbSet<Order> Orders { get; set; }
    }

    public class Gift
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int? NumberAvailable { get; set; }
        public decimal OrderingPosition { get; set; }

        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public string From { get; set; }
        public string? Message { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public Gift Gift { get; set; }
    }
}