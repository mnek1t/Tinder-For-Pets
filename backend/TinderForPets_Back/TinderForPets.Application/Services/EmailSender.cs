using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.Json;
using TinderForPets.Infrastructure;

namespace TinderForPets.Application.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {

            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(Options.SendGridKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "reset-password-template.html");
            var htmlMessage = await File.ReadAllTextAsync(templatePath);
            var emailData = JsonSerializer.Deserialize<ResetPasswordEmailDto>(message);
            htmlMessage = htmlMessage.Replace("{{UserName}}", emailData.UserName);
            htmlMessage = htmlMessage.Replace("{{ResetLink}}", emailData.ResetLink);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.SendGridSenderEmail, Options.SendGridSenderName + " Password Recovery"),
                Subject = subject,
                // TODO: add PlainTextContent property and pass just text 
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html

            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}
