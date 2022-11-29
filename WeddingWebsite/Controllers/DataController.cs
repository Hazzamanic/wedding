using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WeddingWebsite.Data;

namespace WeddingWebsite.Controllers
{
    public class DataController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DataController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("export/guests")]
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users.ToListAsync();
            var userRoles = (await _db.UserRoles.ToListAsync()).Select(e => e.UserId);
            var guests = users.Where(e => !userRoles.Contains(e.Id)).Select(e => new UserExport
            {
                Name = e.Name,
                GuestName = e.GuestName,
                HasResponded = e.HasResponded,
                RespondedAt = e.RespondedAt,
                GuestEmail = e.GuestEmail,
                PartyType = e.PartyType,
                //SaveTheDateAnswer = e.SaveTheDateAnswer,
                //SaveTheDateGuestAnswer = e.SaveTheDateGuestAnswer,
                //HasSentSaveTheDateEmail = e.HasSentSaveTheDateEmail,
                //HasSentGuestSaveTheDateEmail = e.HasSentGuestSaveTheDateEmail,
                Guest1IsAttending = e.Guest1IsAttending,
                Guest1PizzaParty = e.Guest1PizzaParty,
                Guest1Brunch = e.Guest1Brunch,
                Guest1DietaryRequirements = e.Guest1DietaryRequirements,
                Guest1SongRequest = e.Guest1SongRequest,
                Guest1AccommodationList = e.Guest1AccommodationList,
                Guest2IsAttending = e.Guest2IsAttending,
                Guest2PizzaParty = e.Guest2PizzaParty,
                Guest2Brunch = e.Guest2Brunch,
                Guest2DietaryRequirements = e.Guest2DietaryRequirements,
                Guest2SongRequest = e.Guest2SongRequest,
                Guest2AccommodationList = e.Guest2AccommodationList,
                MoreInfoRequest = e.MoreInfoRequest,
            }).OrderBy(e => e.Name).ToList();

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(guests);
            await writer.FlushAsync();

            return File(stream.ToArray(), "text/csv", "guests.csv");
        }
    }

    public class UserExport
    {
        public string? Name { get; set; }

        public string? GuestName { get; set; }

        public bool HasResponded { get; set; }

        public DateTime? RespondedAt { get; set; }

        public string? GuestEmail { get; set; }

        public string PartyType { get; set; }

        public string Guest1IsAttending { get; set; }

        public string Guest1PizzaParty { get; set; }

        public string Guest1Brunch { get; set; }

        public string Guest1DietaryRequirements { get; set; }

        public string Guest1SongRequest { get; set; }

        public string Guest1AccommodationList { get; set; }

        public string Guest2IsAttending { get; set; }

        public string Guest2PizzaParty { get; set; }

        public string Guest2Brunch { get; set; }

        public string Guest2DietaryRequirements { get; set; }

        public string Guest2SongRequest { get; set; }

        public string Guest2AccommodationList { get; set; }

        public string MoreInfoRequest { get; set; }
    }
}
