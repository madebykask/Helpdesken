namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class FieldSettingDto
    {
        public FieldSettingDto(
            string name,
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string bookmark)
        {
            Name = name;
            ShowInDetails = showInDetails;
            ShowInChanges = showInChanges;
            ShowInSelfService = showInSelfService;
            Caption = caption;
            Required = required;
            Bookmark = bookmark;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInChanges { get; private set; }

        public bool ShowInSelfService { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string Bookmark { get; private set; }
    }
}
