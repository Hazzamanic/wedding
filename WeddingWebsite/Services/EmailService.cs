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
        Task<List<EmailResult>> SendEmail(IEnumerable<string> userIds, string emailTemplate, string subject, string? toEmail = null);
        Task<List<EmailResult>> SendSaveTheDate(IEnumerable<string> userIds, string? toEmail = null);
        Task SendGenericEmail(string to, string subject, string htmlBody, string? cc = null);
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
            return await SendEmail(userIds, "SaveTheDate.liquid", SaveTheDateSubject, toEmail);
        }

        public async Task<List<EmailResult>> SendEmail(
            IEnumerable<string> userIds, string emailTemplate, string subject, string? toEmail = null)
        {
            var users = await _db.Users
                .Where(e => userIds.Contains(e.Id))
                .AsNoTracking()
                .ToListAsync();

            var emailTemplatesPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName, "EmailTemplates");

            var template = await File.ReadAllTextAsync(Path.Combine(emailTemplatesPath, emailTemplate));

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
                    LoginUrl = loginUrl,
                    AccommodationName = user.GetAccommodationName(),
                    RoomMate = user.RoomMate
                };

                var cc = string.IsNullOrWhiteSpace(toEmail)
                    ? user.GuestEmail
                    : toEmail;

                var result = new EmailResult { UserId = user.Id, Email = to, Cc = cc };
                await TrySendEmail(to, cc, emailModel, result);

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
                            .Subject(subject)
                            .UsingTemplate(template, emailModel);

                    if (!string.IsNullOrWhiteSpace(cc))
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
        }

        public async Task SendGenericEmail(string to, string subject, string htmlBody, string? cc = null)
        {
            try
            {
                var emailTemplate = "Generic.liquid";
                var emailTemplatesPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName, "EmailTemplates");

                var template = await File.ReadAllTextAsync(Path.Combine(emailTemplatesPath, emailTemplate));

                var email = _fluentEmail
                            .Create()
                            .To(to.Trim())
                            .Subject(subject)
                            .UsingTemplate(template, new GenericEmailModel
                            {
                                Body = htmlBody
                            });

                if (!string.IsNullOrWhiteSpace(cc))
                {
                    email.CC(cc.Trim());
                }

                var result = await email.SendAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private class SaveTheDateEmailModel
        {
            public SaveTheDateEmailModel()
            {

            }

            public string Name { get; set; }
            public string LoginUrl { get; set; }
            public bool IsSharingAccommodation => !string.IsNullOrWhiteSpace(RoomMate);
            public string? RoomMate { get; set; }
            public string AccommodationName { get; set; }
        }

        public class GenericEmailModel
        {
            public string Body { get; set; }
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
