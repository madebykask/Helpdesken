namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LogFieldSettingsOverview
    {
        public LogFieldSettingsOverview(FieldOverviewSetting log)
        {
            this.Log = log;
        }

        [NotNull]
        public FieldOverviewSetting Log { get; private set; }
    }
}