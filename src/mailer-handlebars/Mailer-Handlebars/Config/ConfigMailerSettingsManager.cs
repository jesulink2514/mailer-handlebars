using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;

namespace Mailer.Handlebars.Config
{
    public class ConfigMailerSettingsManager : IMailerSettingsManager
    {
        private readonly MailSettingsSectionGroup _mailSettings;

        public ConfigMailerSettingsManager()
        {
            _mailSettings = (MailSettingsSectionGroup)ConfigurationManager.GetSection("system.net/mailSettings");            
        }

        public SmtpClient CreateSmtpClient()
        {
            var smtp = _mailSettings.Smtp;
            var network = smtp.Network;
            return new SmtpClient(network.Host,network.Port)
            {
                EnableSsl = network.EnableSsl,
                Credentials = new NetworkCredential(network.UserName,network.Password)
            };
        }

        public string From
        {
            get { return _mailSettings.Smtp.From; }
        }
    }
}