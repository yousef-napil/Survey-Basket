using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using Survey_Basket.Abstractions.Settings;

namespace Survey_Basket.Services;

public class EmailSender(IOptions<MailSettings> MailSetting) : IEmailSender
{
    private readonly MailSettings mailSetting = MailSetting.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(mailSetting.Mail),
            Subject = subject
        };
        message.To.Add(MailboxAddress.Parse(email));

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };

        message.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new SmtpClient();
        smtpClient.Connect(mailSetting.Host, mailSetting.Port, SecureSocketOptions.StartTls);
        smtpClient.Authenticate(mailSetting.Mail, mailSetting.Password);
        await smtpClient.SendAsync(message);
        smtpClient.Disconnect(true);
    }
}
