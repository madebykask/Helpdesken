namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public class FieldSettingModel
    {
        public FieldSettingModel()
        {
        }

        public FieldSettingModel(
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string bookmark)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInChanges = showInChanges;
            this.ShowInSelfService = showInSelfService;
            this.Caption = caption;
            this.Required = required;
            this.Bookmark = bookmark;
        }

        public bool ShowInDetails { get; private set; }

        public bool ShowInChanges { get; private set; }

        public bool ShowInSelfService { get; private set; }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; private set; }

        public bool Required { get; private set; }

        public string Bookmark { get; private set; }
    }
}