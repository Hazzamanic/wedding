using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Data;

namespace WeddingWebsite.Pages
{
    public class GiftsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public GiftsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<GiftViewModel> Gifts { get; set; }

        public async Task OnGet()
        {
            var orderByPrice = true;

            var giftsQuery = _db.Gifts
                .Include(e => e.Orders)
                .AsQueryable();

            giftsQuery = orderByPrice
                ? giftsQuery.OrderBy(x => x.Price)
                : giftsQuery.OrderBy(e => e.OrderingPosition);

            var dbGifts = await giftsQuery.ToListAsync();

            var gifts = dbGifts.Select(gift =>
            {
                var pledged = gift.Orders.Sum(x => x.Amount);
                return new GiftViewModel
                {
                    Id = gift.Id,
                    Title = gift.Title,
                    Description = gift.Description,
                    ImageUrl = gift.ImageUrl,
                    Price = gift.Price,

                    Pledged = pledged,
                };
            }).ToList();

            Gifts = gifts;
        }

        public class GiftViewModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public decimal Price { get; set; }
            public int? NumberAvailable { get; set; }
            public decimal Pledged { get; set; }
            public bool IsAvilable { get; set; }
        }
    }
}
