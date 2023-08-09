using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string content);
}

public class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;

    public EmailService(ISendGridClient sendGridClient)
    {
        _sendGridClient = sendGridClient;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string content)
    {
        var from = new EmailAddress("IT-PortalenAdmin@se.issworld.com", "MasterSiteList Admin");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        await _sendGridClient.SendEmailAsync(msg);
    }
}
