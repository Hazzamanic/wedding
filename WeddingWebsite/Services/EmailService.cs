using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;
using WeddingWebsite.Extensions;

namespace WeddingWebsite.Services
{
    public interface IEmailService
    {
        Task<List<EmailResult>> SendSaveTheDate(IEnumerable<string> userIds, string? toEmail = null);
    }

    public class EmailService : IEmailService
    {
        const string SaveTheDateSubject = "Real RSVP - Harry & Sinead are getting married!";

        private readonly ApplicationDbContext _db;
        private readonly IFluentEmail _fluentEmail;

        public EmailService(ApplicationDbContext db, IFluentEmail fluentEmail)
        {
            _db = db;
            _fluentEmail = fluentEmail;
        }

        public async Task<List<EmailResult>> SendSaveTheDate(IEnumerable<string> userIds, string? toEmail = null)
        {
            var users = await _db.Users
                .Where(e => userIds.Contains(e.Id))
                .AsNoTracking()
                .ToListAsync();

            var emailTemplatesPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName, "EmailTemplates");

            var template = await File.ReadAllTextAsync(Path.Combine(emailTemplatesPath, "SaveTheDate.liquid"));

            var results = new List<EmailResult>();

            var hasToEmail = !string.IsNullOrWhiteSpace(toEmail);

            foreach (var user in users)
            {
                var loginUrl = $"https://www.harrygetsknighted.com/dr?code={Uri.UnescapeDataString(user.DirectLoginCode)}";

                var to = string.IsNullOrWhiteSpace(toEmail)
                    ? user.Email
                    : toEmail;

                var emailModel = new SaveTheDateEmailModel
                {
                    Name = user.GetGroupName(),
                    LoginUrl = loginUrl
                };

                var result = new EmailResult { UserId = user.Id, Email = user.Email };
                await TrySendEmail(to, emailModel, result);
                if (!hasToEmail && result.IsSuccess)
                {
                    await MarkSaveTheDateSent(user);
                }
                results.Add(result);

                if (!string.IsNullOrWhiteSpace(user.GuestEmail))
                {
                    var toGuest = string.IsNullOrWhiteSpace(toEmail)
                        ? user.GuestEmail
                        : toEmail;

                    var guestResult = new EmailResult { UserId = user.Id, Email = user.GuestEmail };
                    await TrySendEmail(toGuest, emailModel, guestResult);
                    if (!hasToEmail && guestResult.IsSuccess)
                    {
                        await MarkSaveTheDateSentForGuest(user);
                    }

                    results.Add(guestResult);
                }
            }

            return results;

            async Task TrySendEmail(string to, SaveTheDateEmailModel emailModel, EmailResult emailResult)
            {
                try
                {
                    var result = await _fluentEmail
                            .To(to.Trim())
                            .Subject(SaveTheDateSubject)
                            .UsingTemplate(template, emailModel)
                            .SendAsync();

                    emailResult.IsSuccess = result.Successful;
                    emailResult.ErrorMessage = result.ErrorMessages?.Any() == true ? String.Join(',', result.ErrorMessages) : null;
                }
                catch (Exception ex)
                {
                    emailResult.IsSuccess = false;
                    emailResult.ErrorMessage = ex.Message;
                }
            }

            async Task MarkSaveTheDateSent(User user)
            {
                user.HasSentSaveTheDateEmail = true;
                _db.Users.Update(user);
               // _db.Entry(user).Property(x => x.HasSentSaveTheDateEmail).IsModified = true;
                await _db.SaveChangesAsync();
            }

            async Task MarkSaveTheDateSentForGuest(User user)
            {
                user.HasSentGuestSaveTheDateEmail = true;
                _db.Users.Update(user);
                // _db.Entry(user).Property(x => x.HasSentSaveTheDateEmail).IsModified = true;
                await _db.SaveChangesAsync();
            }
        }

        private class SaveTheDateEmailModel
        {
            public SaveTheDateEmailModel()
            {

            }

            public string Name { get; set; }
            public string LoginUrl { get; set; }
        }
    }

    public class EmailResult
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
