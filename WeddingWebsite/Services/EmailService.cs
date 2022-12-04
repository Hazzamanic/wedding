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
        private readonly IFluentEmailFactory _fluentEmail;

        public EmailService(ApplicationDbContext db, IFluentEmailFactory fluentEmail)
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

                var cc = string.IsNullOrWhiteSpace(toEmail)
                    ? user.GuestEmail
                    : toEmail;

                var result = new EmailResult { UserId = user.Id, Email = to, Cc = cc };
                await TrySendEmail(to, cc, emailModel, result);

                if (!hasToEmail && result.IsSuccess)
                {
                    await MarkSaveTheDateSent(user);
                }
                results.Add(result);
            }

            return results;

            async Task TrySendEmail(string to, string? cc, SaveTheDateEmailModel emailModel, EmailResult emailResult)
            {
                try
                {
                    var email = _fluentEmail
                            .Create()
                            .To(to.Trim())
                            .Subject(SaveTheDateSubject)
                            .UsingTemplate(template, emailModel);

                    if(!string.IsNullOrWhiteSpace(cc))
                    {
                        email.CC(cc.Trim());
                    }

                    var result = await email.SendAsync();

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
        public string Cc { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
