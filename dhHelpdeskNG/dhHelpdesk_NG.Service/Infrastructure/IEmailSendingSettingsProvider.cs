namespace DH.Helpdesk.Services.Infrastructure.SettingProviders
{
    public interface IEmailSendingSettingsProvider
    {
        EmailSendingSettings GetSettings();
    }
}