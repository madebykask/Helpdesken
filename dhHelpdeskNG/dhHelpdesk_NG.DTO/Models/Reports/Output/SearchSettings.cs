namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchSettings
    {
        public SearchSettings(FieldOverviewSetting reports)
        {
            this.Reports = reports;
        }

        [NotNull]
        public FieldOverviewSetting Reports { get; private set; }
    }
}