namespace WEBDOAN.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using (var client = new SmtpClient(_settings.Host, _settings.Port))
        {
            client.EnableSsl = _settings.EnableSsl;
            client.Credentials = new NetworkCredential(_settings.UserName, _settings.Password);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.UserName, "Đặt Là Có Ăn Là Ghiền"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}
