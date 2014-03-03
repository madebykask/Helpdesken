namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralProcessingSettings
    {
        public GeneralProcessingSettings(
            FieldProcessingSetting priority,
            FieldProcessingSetting title,
            FieldProcessingSetting status,
            FieldProcessingSetting system,
            FieldProcessingSetting @object,
            FieldProcessingSetting workingGroup,
            FieldProcessingSetting administrator,
            FieldProcessingSetting finishingDate,
            FieldProcessingSetting rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.Status = status;
            this.System = system;
            this.Object = @object;
            this.WorkingGroup = workingGroup;
            this.Administrator = administrator;
            this.FinishingDate = finishingDate;
            this.Rss = rss;
        }

        [NotNull]
        public FieldProcessingSetting Priority { get; private set; }

        [NotNull]
        public FieldProcessingSetting Title { get; private set; }

        [NotNull]
        public FieldProcessingSetting Status { get; private set; }

        [NotNull]
        public FieldProcessingSetting System { get; private set; }

        [NotNull]
        public FieldProcessingSetting Object { get; private set; }

        [NotNull]
        public FieldProcessingSetting WorkingGroup { get; private set; }

        [NotNull]
        public FieldProcessingSetting Administrator { get; private set; }

        [NotNull]
        public FieldProcessingSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldProcessingSetting Rss { get; private set; }
    }
}
