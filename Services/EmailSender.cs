using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GameSite.Services;

public class EmailSender : IEmailSender
{
    ILogger<EmailSender> logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        this.logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //TODO Зробити підтвердження по пошті
        logger.LogDebug(htmlMessage);

        await Task.CompletedTask;

        // var client = new SendGridClient(sendGridApiKey);
        // var from = new EmailAddress("your-email@example.com", "GameSiteUa");
        // var to = new EmailAddress(email);
        // var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
        // var response = await client.SendEmailAsync(msg);
        // if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
        // Обробка помилок
    }
}
