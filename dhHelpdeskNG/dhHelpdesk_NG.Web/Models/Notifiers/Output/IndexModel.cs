namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class IndexModel
    {
        public IndexModel(NotifiersModel notifiers, SettingsModel settings)
        {
            this.Notifiers = notifiers;
            this.Settings = settings;
        }

        [NotNull]
        public SettingsModel Settings { get; private set; }

        [NotNull]
        public NotifiersModel Notifiers { get; private set; }
    }
}