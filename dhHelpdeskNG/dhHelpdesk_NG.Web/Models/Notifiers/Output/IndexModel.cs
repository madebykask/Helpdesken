namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class IndexModel
    {
        public IndexModel(NotifiersModel notifiers, SettingsModel settings)
        {
            ArgumentsValidator.NotNull(notifiers, "notifiers");
            ArgumentsValidator.NotNull(settings, "settings");

            this.Notifiers = notifiers;
            this.Settings = settings;
        }

        public SettingsModel Settings { get; private set; }

        public NotifiersModel Notifiers { get; private set; }
    }
}