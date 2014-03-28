namespace DH.Helpdesk.Services.Infrastructure.SettingProviders.Concrete
{
    using System.Configuration;

    using DH.Helpdesk.Common.Enums;

    public sealed class EmailSendingSettingsProvider : IEmailSendingSettingsProvider
    {
        public EmailSendingSettings GetSettings()
        {
            var smtpServer = ConfigurationManager.AppSettings[AppSettingsKey.SmtpServer];
            var smtpPort = ConfigurationManager.AppSettings[AppSettingsKey.SmtpPort];

            var smtpPortNumber = int.Parse(smtpPort);

            return new EmailSendingSettings(smtpPortNumber, smtpServer);
        }
    }
}