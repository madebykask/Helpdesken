﻿namespace DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralFieldSettings
    {
        public GeneralFieldSettings(
            FieldSetting priority,
            FieldSetting title,
            FieldSetting status,
            FieldSetting system,
            FieldSetting @object,
            FieldSetting inventory,
            FieldSetting workingGroup,
            FieldSetting administrator,
            FieldSetting finishingDate,
            FieldSetting rss)
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
        public FieldSetting Priority { get; private set; }

        [NotNull]
        public FieldSetting Title { get; private set; }

        [NotNull]
        public FieldSetting Status { get; private set; }

        [NotNull]
        public FieldSetting System { get; private set; }

        [NotNull]
        public FieldSetting Object { get; private set; }

        [NotNull]
        public FieldSetting Inventory { get; private set; }

        [NotNull]
        public FieldSetting WorkingGroup { get; private set; }

        [NotNull]
        public FieldSetting Administrator { get; private set; }

        [NotNull]
        public FieldSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldSetting Rss { get; private set; }
    }
}
