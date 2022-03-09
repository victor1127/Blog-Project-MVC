using BlogProjectMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Services
{
    public class EmailService : IBlogEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        public Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
