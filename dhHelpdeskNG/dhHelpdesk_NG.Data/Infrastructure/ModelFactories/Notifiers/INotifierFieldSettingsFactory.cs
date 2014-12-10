namespace DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;

    public interface INotifierFieldSettingsFactory
    {
        FieldSetting Create(
                    bool showInDetails,
                    bool showInNotifiers,
                    string caption,
                    string lableText,
                    bool required,
                    string ldapAttribute);        
        
        FieldSetting CreateEmpty();
    }
}