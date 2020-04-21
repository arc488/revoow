using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Areas.Identity
{
    public class EmailSender : IEmailSender
    {
        private readonly string apiKey;
        private readonly string fromEmail;
        private readonly string fromName;

        public EmailSender(IConfiguration configuration)
        {
            this.apiKey = configuration["SendGrid:ApiKey"];
            this.fromEmail = configuration["SendGrid:FromEmail"];
            this.fromName = configuration["SendGrid:FromName"];
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(this.apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(this.fromEmail, this.fromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            await client.SendEmailAsync(msg);
        }
    }
}
