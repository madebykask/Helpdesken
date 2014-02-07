namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class UpdatedFieldSettingDto
    {
        public UpdatedFieldSettingDto(
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string bookmark,
            DateTime changedDateTime)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInChanges = showInChanges;
            this.ShowInSelfService = showInSelfService;
            this.Caption = caption;
            this.Required = required;
            this.Bookmark = bookmark;
            this.ChangedDateTime = changedDateTime;
        }

        public bool ShowInDetails { get; private set; }

        public bool ShowInChanges { get; private set; }

        public bool ShowInSelfService { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string Bookmark { get; private set; }

        public DateTime ChangedDateTime { get; private set; }
    }
}
