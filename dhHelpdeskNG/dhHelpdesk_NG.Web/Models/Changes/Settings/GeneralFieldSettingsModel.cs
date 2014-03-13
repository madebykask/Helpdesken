namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public sealed class GeneralFieldSettingsModel
    {
        public GeneralFieldSettingsModel()
        {
        }

        public GeneralFieldSettingsModel(
            FieldSettingModel priority,
            FieldSettingModel title,
            FieldSettingModel status,
            FieldSettingModel system,
            FieldSettingModel @object,
            FieldSettingModel inventory,
            FieldSettingModel workingGroup,
            FieldSettingModel administrator,
            FieldSettingModel finishingDate,
            FieldSettingModel rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.Status = status;
            this.System = system;
            this.Object = @object;
            this.Inventory = inventory;
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
        [LocalizedDisplay("Status")]
        public FieldSettingModel Status { get; set; }

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
        [LocalizedDisplay("Working Group")]
        public FieldSettingModel WorkingGroup { get; set; }

        [NotNull]
        [LocalizedDisplay("Administrator")]
        public FieldSettingModel Administrator { get; set; }

        [NotNull]
        [LocalizedDisplay("Finishing Date")]
        public FieldSettingModel FinishingDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Rss")]
        public FieldSettingModel Rss { get; set; }
    }
}
