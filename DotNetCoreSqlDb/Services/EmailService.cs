using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace DotNetCoreSqlDb.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailService(IConfiguration configuration)
        {
            string sendGridApiKey = configuration["SendGridApiKey"];
            _sendGridClient = new SendGridClient(sendGridApiKey);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var from = new EmailAddress("IT-PortalenAdmin@se.issworld.com", "MasterSiteList Admin");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            await _sendGridClient.SendEmailAsync(msg);
        }
    } 
} 
