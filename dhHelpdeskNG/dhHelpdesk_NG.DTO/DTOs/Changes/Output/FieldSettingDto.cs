namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public class FieldSettingDto
    {
        public FieldSettingDto(
            string name,
            bool showInDetails,
            bool showInChanges,
            bool showInChangesService,
            string caption,
            bool required,
            string bookmark)
        {
            ArgumentsValidator.NotNullAndEmpty(name, "name");
            ArgumentsValidator.NotNullAndEmpty(caption, "caption");

            Name = name;
            ShowInDetails = showInDetails;
            ShowInChanges = showInChanges;
            ShowInChangesService = showInChangesService;
            Caption = caption;
            Required = required;
            Bookmark = bookmark;
        }

        public string Name { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInChanges { get; private set; }

        public bool ShowInChangesService { get; private set; }

        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string Bookmark { get; private set; }
    }
}
