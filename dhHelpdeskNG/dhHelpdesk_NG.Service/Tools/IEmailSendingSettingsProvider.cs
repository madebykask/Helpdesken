namespace DH.Helpdesk.Services.Tools
{
    public interface IEmailSendingSettingsProvider
    {
        EmailSendingSettings GetSettings();
    }
}