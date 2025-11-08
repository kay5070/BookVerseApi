using System.Net;
using System.Net.Mail;
using BookStoreApi.Interfaces;

namespace BookStoreApi.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpSettings = _config.GetSection("SmtpSettings");
        var from = smtpSettings["From"];
        var host = smtpSettings["Host"];
        var port = int.Parse(smtpSettings["Port"]!);
        var username = smtpSettings["Username"];
        var password = smtpSettings["Password"];
        var displayName = smtpSettings["DisplayName"] ?? from;
        using var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(username, password)
        };
        var mail = new MailMessage
        {
            From = new MailAddress(from!,displayName),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };
        mail.To.Add(to);
        await client.SendMailAsync(mail);
    }
}