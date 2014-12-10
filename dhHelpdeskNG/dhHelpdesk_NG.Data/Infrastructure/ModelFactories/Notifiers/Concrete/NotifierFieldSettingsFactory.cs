namespace DH.Helpdesk.Dal.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;

    public sealed class NotifierFieldSettingsFactory : INotifierFieldSettingsFactory
    {
        public FieldSetting Create(
            bool showInDetails,
            bool showInNotifiers,
            string caption,
            string lableText,
            bool required,
            string ldapAttribute)
        {
            var instance = new FieldSetting(
                                showInDetails,
                                showInNotifiers,
                                caption,
                                lableText,
                                required,
                                ldapAttribute);
            return instance;
        }

        public FieldSetting CreateEmpty()
        {
            var empty = new FieldSetting(
                            false,
                            false,
                            "Empty",
                            "Empty",
                            false,
                            string.Empty);
            empty.MarkAsEmpty();
            return empty;
        }
    }
}