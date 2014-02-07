namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

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