namespace DH.Helpdesk.Services.Infrastructure
{
    public interface IEmailSendingSettingsProvider
    {
        EmailSendingSettings GetSettings();
    }
}