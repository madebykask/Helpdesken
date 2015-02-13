namespace DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ComputerSettings
    {
        public ComputerSettings(
            FieldOverviewSetting number, 
            FieldOverviewSetting computerType, 
            FieldOverviewSetting place)
        {
            this.Place = place;
            this.ComputerType = computerType;
            this.PcNumber = number;
        }

        [NotNull]
        public FieldOverviewSetting PcNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting ComputerType { get; private set; }

        [NotNull]
        public FieldOverviewSetting Place { get; private set; }
    }
}