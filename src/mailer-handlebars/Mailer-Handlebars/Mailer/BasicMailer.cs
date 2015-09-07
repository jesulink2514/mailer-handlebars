using System;
using System.Diagnostics;
using System.Net.Mail;
using Mailer.Handlebars.Config;
using Mailer.Handlebars.Template;

namespace Mailer.Handlebars.Mailer
{
    public class BasicMailer
    {
        private readonly IEmailFormatter _formatter;
        private readonly IMailerSettingsManager _mailerSettingsManager;

        public BasicMailer
            (
            IEmailFormatter formatter,
            IMailerSettingsManager mailerSettingsManager)
        {
            _formatter = formatter;
            _mailerSettingsManager = mailerSettingsManager;
        }

        public EmailStatus EnviarCorreo<T>(string to, string subject, string templateName, T data)
        {
            var htmlContent = _formatter.GetContent(templateName, data);
            return SendEmailText(to, subject, htmlContent);
        }

        private EmailStatus SendEmailText(string to, string subject, string htmlContent)
        {
            var client = _mailerSettingsManager.CreateSmtpClient();
            var from = _mailerSettingsManager.From;

            using (var message = new MailMessage(@from, to, subject, htmlContent) {IsBodyHtml = true})
            {
                try
                {
                    client.Send(message);
                }
                catch (SmtpFailedRecipientException e)
                {
                    return EmailStatus.NotSended;
                }
                catch (SmtpException ex)
                {
                    switch (ex.StatusCode)
                    {
                        case SmtpStatusCode.BadCommandSequence:
                        case SmtpStatusCode.MailboxNameNotAllowed:
                        case SmtpStatusCode.HelpMessage:
                        case SmtpStatusCode.SyntaxError:
                        case SmtpStatusCode.SystemStatus:
                        case SmtpStatusCode.CannotVerifyUserWillAttemptDelivery:
                        case SmtpStatusCode.UserNotLocalWillForward:
                            return EmailStatus.Error;
                        case SmtpStatusCode.ClientNotPermitted:
                        case SmtpStatusCode.CommandNotImplemented:
                        case SmtpStatusCode.CommandParameterNotImplemented:
                        case SmtpStatusCode.CommandUnrecognized:
                        case SmtpStatusCode.ExceededStorageAllocation:
                        case SmtpStatusCode.GeneralFailure:
                        case SmtpStatusCode.InsufficientStorage:
                        case SmtpStatusCode.LocalErrorInProcessing:
                        case SmtpStatusCode.MailboxBusy:
                        case SmtpStatusCode.MailboxUnavailable:
                        case SmtpStatusCode.MustIssueStartTlsFirst:
                        case SmtpStatusCode.ServiceClosingTransmissionChannel:
                        case SmtpStatusCode.ServiceNotAvailable:
                        case SmtpStatusCode.ServiceReady:
                        case SmtpStatusCode.StartMailInput:
                        case SmtpStatusCode.TransactionFailed:
                        case SmtpStatusCode.UserNotLocalTryAlternatePath:
                            return EmailStatus.NotSended;
                        case SmtpStatusCode.Ok:
                            return EmailStatus.Sended;
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                    return EmailStatus.Error;
                }
            }
            return EmailStatus.Sended;
        }
    }
}
