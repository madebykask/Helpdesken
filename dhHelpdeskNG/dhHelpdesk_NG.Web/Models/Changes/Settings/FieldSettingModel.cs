namespace DH.Helpdesk.Web.Models.Changes.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;

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

        public bool ShowInDetails { get; set; }

        public bool ShowInChanges { get; set; }

        public bool ShowInSelfService { get; set; }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; set; }

        public bool Required { get; set; }

        public string Bookmark { get; set; }
    }
}