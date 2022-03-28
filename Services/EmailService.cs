using BlogProjectMVC.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Services
{
    public class EmailService : IBlogEmailSender
    {
        private readonly EmailSettingsModel _emailSettings;
        public EmailService(IOptions<EmailSettingsModel> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(_emailSettings.Email));
            email.Subject = subject;

            var emailBody = new BodyBuilder()
            {
                HtmlBody = $"New message received from:<br/><br/>" +
                           $"Name: { name }<br/>" +
                           $"Email: { emailFrom }<br/>" +
                           $"Subject: { subject }<br/>" +
                           $"<hr/><br/> { htmlMessage }"
                
            };

            email.Body = emailBody.ToMessageBody();

            var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
            
            await smtp.SendAsync(email);
            smtp.Disconnect(true);


        }

        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;

            var emailBody = new BodyBuilder()
            {
                HtmlBody = htmlMessage
            };

            email.Body = emailBody.ToMessageBody();

            //Configure SMTP client that will sent the email off

            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }                          
    }
}
