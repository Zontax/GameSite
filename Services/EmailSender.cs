using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
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
        // Set up SMTP client
        SmtpClient client = new SmtpClient("smtp.ethereal.email", 587);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential("ua.game.world.ex@gmail.com", "djgwPz8DPss78aakBv");

        // Create email message
        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("ua.game.world.ex@gmail.com");
        mailMessage.To.Add(email);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;

        var mailBody = new StringBuilder();
        mailBody.AppendFormat("<h1>User Registered</h1>");
        mailBody.AppendFormat("<br />");
        mailBody.AppendFormat("<p>Thank you For Registering account</p>");
        mailBody.AppendFormat(htmlMessage);

        mailMessage.Body = mailBody.ToString();

        client.Send(mailMessage);

        logger.LogDebug(htmlMessage);
        await Task.CompletedTask;
    }
}
