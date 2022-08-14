using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WeddingWebsite.Data;
using WeddingWebsite.Data.Entities;

namespace WeddingWebsite.Services
{
    public interface IEmailService
    {
        Task SendInvite(IEnumerable<string> userIds);
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

        public async Task SendInvite(IEnumerable<string> userIds)
        {
            var users = await _db.Users.Where(e => userIds.Contains(e.Id)).ToListAsync();

            var emailTemplatesPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName, "EmailTemplates");

            var template = await File.ReadAllTextAsync(Path.Combine(emailTemplatesPath, "SaveTheDate.liquid"));

            foreach (var user in users)
            {
                await _fluentEmail
                    .To(user.Email)
                    .UsingTemplate(template, new SaveTheDateEmailModel(user))
                    .SendAsync();
            }
        }

        private class SaveTheDateEmailModel
        {
            public SaveTheDateEmailModel(User user)
            {
                Name = user.Name;
            }

            public string Name { get; set; }
        }
    }
}
