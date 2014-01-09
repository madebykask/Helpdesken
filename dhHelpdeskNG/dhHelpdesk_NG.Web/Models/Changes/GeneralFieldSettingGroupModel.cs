namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class GeneralFieldSettingGroupModel
    {
        public GeneralFieldSettingGroupModel()
        {
        }

        public GeneralFieldSettingGroupModel(
            FieldSettingModel priority,
            FieldSettingModel title,
            FieldSettingModel state,
            FieldSettingModel system,
            FieldSettingModel @object,
            FieldSettingModel inventory,
            FieldSettingModel owner,
            FieldSettingModel workingGroup,
            FieldSettingModel administrator,
            FieldSettingModel finishingDate,
            FieldSettingModel rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.State = state;
            this.System = system;
            this.Object = @object;
            this.Inventory = inventory;
            this.Owner = owner;
            this.WorkingGroup = workingGroup;
            this.Administrator = administrator;
            this.FinishingDate = finishingDate;
            this.Rss = rss;
        }

        [NotNull]
        [LocalizedDisplay("Priority")]
        public FieldSettingModel Priority { get; set; }

        [NotNull]
        [LocalizedDisplay("Title")]
        public FieldSettingModel Title { get; set; }

        [NotNull]
        [LocalizedDisplay("State")]
        public FieldSettingModel State { get; set; }

        [NotNull]
        [LocalizedDisplay("System")]
        public FieldSettingModel System { get; set; }

        [NotNull]
        [LocalizedDisplay("Object")]
        public FieldSettingModel Object { get; set; }

        [NotNull]
        [LocalizedDisplay("Inventory")]
        public FieldSettingModel Inventory { get; set; }

        [NotNull]
        [LocalizedDisplay("Owner")]
        public FieldSettingModel Owner { get; set; }

        [NotNull]
        [LocalizedDisplay("Working group")]
        public FieldSettingModel WorkingGroup { get; set; }

        [NotNull]
        [LocalizedDisplay("Administrator")]
        public FieldSettingModel Administrator { get; set; }

        [NotNull]
        [LocalizedDisplay("Finishing date")]
        public FieldSettingModel FinishingDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Rss")]
        public FieldSettingModel Rss { get; set; }
    }
}
