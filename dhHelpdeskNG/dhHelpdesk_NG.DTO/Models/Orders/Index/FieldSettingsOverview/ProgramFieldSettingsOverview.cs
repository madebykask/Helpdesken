namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramFieldSettingsOverview
    {
        public ProgramFieldSettingsOverview(FieldOverviewSetting program)
        {
            this.Program = program;
        }

        [NotNull]
        public FieldOverviewSetting Program { get; private set; }         
    }
}