namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class FieldSettingModel
    {
        public FieldSettingModel(
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string bookmark)
        {
            ShowInDetails = showInDetails;
            ShowInChanges = showInChanges;
            ShowInSelfService = showInSelfService;
            Caption = caption;
            Required = required;
            Bookmark = bookmark;
        }

        public bool ShowInDetails { get; private set; }

        public bool ShowInChanges { get; private set; }

        public bool ShowInSelfService { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string Bookmark { get; private set; }
    }
}