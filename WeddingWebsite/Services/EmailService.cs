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
        Task SendSaveTheDate(IEnumerable<string> userIds, string? toEmail = null);
    }

    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _db;
        private readonly IFluentEmail _fluentEmail;

        public EmailService(ApplicationDbContext db, IFluentEmail fluentEmail)
        {
            _db = db;
            _fluentEmail = fluentEmail;
        }

        public async Task SendSaveTheDate(IEnumerable<string> userIds, string? toEmail = null)
        {
            var users = await _db.Users.Where(e => userIds.Contains(e.Id)).ToListAsync();

            var emailTemplatesPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName, "EmailTemplates");

            var template = await File.ReadAllTextAsync(Path.Combine(emailTemplatesPath, "SaveTheDate.liquid"));

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

                await _fluentEmail
                    .To(to)
                    .UsingTemplate(template, emailModel)
                    .SendAsync();

                if (!string.IsNullOrWhiteSpace(user.GuestEmail))
                {
                    var toGuest = string.IsNullOrWhiteSpace(toEmail)
                        ? user.GuestEmail
                        : toEmail;

                    await _fluentEmail
                        .To(toGuest)
                        .UsingTemplate(template, emailModel)
                        .SendAsync();
                }
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
}
