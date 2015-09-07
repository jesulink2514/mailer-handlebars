using System.Net.Mail;

namespace Mailer.Handlebars.Config
{
    public interface IMailerSettingsManager
    {
        SmtpClient CreateSmtpClient();
        string From { get; }
    }
}