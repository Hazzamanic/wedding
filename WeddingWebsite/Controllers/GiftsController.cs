using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingWebsite.Data;
using WeddingWebsite.Services;

namespace WeddingWebsite.Controllers;

[Route("/Gifts/{action=Index}/{id?}")]
public class GiftsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService;

    public GiftsController(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        var orderByPrice = true;

        var giftsQuery = _db.Gifts
            .Include(e => e.Orders)
            .AsNoTracking()
            .AsQueryable();

        giftsQuery = orderByPrice
            ? giftsQuery.OrderBy(x => x.Price == 0).ThenBy(x => x.Price)
            : giftsQuery.OrderBy(e => e.OrderingPosition);

        var dbGifts = await giftsQuery.ToListAsync();

        var gifts = dbGifts.Select(gift =>
        {
            return Map(gift);
        }).ToList();

        var vm = new GiftListViewModel
        {
            Gifts = gifts
        };

        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        return await BuildDetailsView(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind(Prefix = nameof(GiftViewModel.Order))] CreateOrderRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return await BuildDetailsView(request.GiftId, request);
        }

        var dbGift = await _db.Gifts
            .Include(e => e.Orders)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.GiftId);

        if (dbGift == null)
        {
            return NotFound();
        }

        var order = new Order
        {
            GiftId = request.GiftId,
            Amount = request.ContributionAmount,
            CreatedAtUtc = DateTime.UtcNow,
            From = request.Name,
            Message = request.Message
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        try
        {
            // send email if populated
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var body =
                    $"Dear {request.Name}, <br /><br />" +
                    $"Thank you so much for your contribution of £{request.ContributionAmount}! We will be sure to send you some photos from the honeymoon of us enjoying {dbGift.Title}!.<br /><br />" +
                    $"You can pay via bank transfer or <a href=\"https://monzo.me/harrywestbrook/{request.ContributionAmount}\">Monzo</a> <br />" +
                    "Name: Sinead Westbrook-Knight <br />" +
                    "Account Number: 9935 6764 <br />" +
                    "Sort Code: 04 00 04 <br /> <br />" +
                    "Thank you again,<br /> Harry & Sinead x";

                await _emailService.SendGenericEmail(request.Email, "Thank you for the gift!", body);
            }

            // send us an email
            var alertBody =
                $"{request.Name} just contributed £{request.ContributionAmount} towards {dbGift.Title}.";

            await _emailService.SendGenericEmail("sinead_knight@hotmail.com", "New gift!", alertBody, "breakitup@hotmail.co.uk");
        }

        catch (Exception) { }


        return RedirectToAction(nameof(Order), new
        {
            id = order.Id
        });
    }

    [HttpGet]
    public async Task<IActionResult> Order(int id)
    {
        var order = await _db.Orders
            .Include(e => e.Gift)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        var vm = new OrderConfirmationViewModel
        {
            GiftTitle = order.Gift.Title,
            GiftImageUrl = order.Gift.ImageUrl,

            ContributionAmount = order.Amount,
            From = order.From,
            Message = order.Message,
        };

        return View("Confirmation", vm);
    }

    private async Task<IActionResult> BuildDetailsView(int id, CreateOrderRequest? request = null)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var dbGift = await _db.Gifts
            .Include(e => e.Orders)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        if (dbGift == null)
        {
            return NotFound();
        }

        var vm = Map(dbGift, request);
        return View(vm);
    }

    private static GiftViewModel Map(Gift gift, CreateOrderRequest? request = null)
    {
        var pledged = gift.Orders.Sum(x => x.Amount);
        var isAvailable =
            gift.NumberAvailable is null or 0
            || (gift.NumberAvailable * gift.Price) > pledged;
        var pledgedToASingleItem = pledged == 0 ? 0 : pledged % gift.Price;

        var totalAvailable = gift.NumberAvailable is null or 0 || pledged == 0
            ? gift.NumberAvailable
            : (int)Math.Ceiling(gift.NumberAvailable.Value - (Math.Min(pledged, gift.Price * gift.NumberAvailable.Value) / gift.Price));

        return new GiftViewModel
        {
            Id = gift.Id,
            Title = gift.Title,
            Description = gift.Description,
            ImageUrl = gift.ImageUrl,
            Price = gift.Price,
            NumberAvailable = totalAvailable,

            Pledged = pledged,
            IsAvilable = isAvailable,
            PledgedToAnItem = pledgedToASingleItem,

            Order = request ?? new()
            {
                ContributionAmount = gift.Price - pledgedToASingleItem,
                GiftId = gift.Id,
            }
        };
    }
}

public class GiftListViewModel
{
    public List<GiftViewModel> Gifts { get; set; }
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
    public decimal PledgedToAnItem { get; set; }

    public CreateOrderRequest Order { get; set; } = new();
}

public class CreateOrderRequest
{
    public int GiftId { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? Message { get; set; }
    [Required]
    [Range(0, double.PositiveInfinity)]
    public decimal ContributionAmount { get; set; }
}

public class OrderConfirmationViewModel
{
    public string GiftTitle { get; set; }
    public string GiftImageUrl { get; set; }
    public decimal ContributionAmount { get; set; }
    public string From { get; set; }
    public string? Email { get; set; }
    public string? Message { get; set; }
}