namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using System;

    using dhHelpdesk_NG.Common.ValidationAttributes;

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
            ShowInDetails = showInDetails;
            ShowInChanges = showInChanges;
            ShowInSelfService = showInSelfService;
            Caption = caption;
            Required = required;
            Bookmark = bookmark;
            ChangedDateTime = changedDateTime;
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
