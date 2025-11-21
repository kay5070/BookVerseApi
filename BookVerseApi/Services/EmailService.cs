using System.Net;
using System.Net.Mail;
using BookVerseApi.Interfaces;
using BookVerseApi.Models;
using Microsoft.Extensions.Options;

namespace BookVerseApi.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<EmailService> _logger;

    // private readonly IConfiguration _config;
    //
    // public EmailService(IConfiguration config)
    // {
    //     _config = config;
    // }
    public EmailService(IOptions<EmailOptions> emailOptions,ILogger<EmailService> logger)
    {
        _emailOptions = emailOptions.Value;
        _logger = logger;
    }
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_emailOptions.FromEmail, _emailOptions.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            message.To.Add(toEmail);

            using var smtp = new SmtpClient(_emailOptions.SmtpHost, _emailOptions.SmtpPort)
            {
                Credentials = new NetworkCredential(_emailOptions.SmtpUsername, _emailOptions.SmtpPassword),
                EnableSsl = true
            };
            await smtp.SendMailAsync(message);
            _logger.LogInformation($"Email send successfully to {toEmail}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"Failed to send email to{toEmail}.");
        }
    }
}