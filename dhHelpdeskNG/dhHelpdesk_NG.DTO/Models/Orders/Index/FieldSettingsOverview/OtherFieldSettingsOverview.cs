namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OtherFieldSettingsOverview
    {
        public OtherFieldSettingsOverview(
                FieldOverviewSetting fileName, 
                FieldOverviewSetting caseNumber, 
                FieldOverviewSetting info, 
                FieldOverviewSetting status)
        {
            this.Status = status;
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
        }

        [NotNull]
        public FieldOverviewSetting FileName { get; private set; }

        [NotNull]
        public FieldOverviewSetting CaseNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting Info { get; private set; }

        [NotNull]
        public FieldOverviewSetting Status { get; private set; }         
    }
}