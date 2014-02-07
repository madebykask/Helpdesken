namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangesOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchFieldSettings
    {
        public SearchFieldSettings(
            FieldOverviewSetting status,
            FieldOverviewSetting @object,
            FieldOverviewSetting workingGroup,
            FieldOverviewSetting administrator)
        {
            this.Status = status;
            this.Object = @object;
            this.WorkingGroup = workingGroup;
            this.Administrator = administrator;
        }

        [NotNull]
        public FieldOverviewSetting Status { get; private set; }

        [NotNull]
        public FieldOverviewSetting Object { get; private set; }

        [NotNull]
        public FieldOverviewSetting WorkingGroup { get; private set; }

        [NotNull]
        public FieldOverviewSetting Administrator { get; private set; }
    }
}
