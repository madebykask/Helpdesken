namespace DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSetting
    {
        protected FieldSetting()
        {
        }

        #region Public Properties

        public string Bookmark { get; protected set; }

        [NotNullAndEmpty]
        public string Caption { get; protected set; }

        public bool Required { get; protected set; }

        public bool ShowInChanges { get; protected set; }

        public bool ShowInDetails { get; protected set; }

        public bool ShowInSelfService { get; protected set; }

        #endregion

        #region Public Methods and Operators

        public static FieldSetting CreateUpdated(
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string bookmark)
        {
            return new FieldSetting
                       {
                           ShowInDetails = showInDetails,
                           ShowInChanges = showInChanges,
                           ShowInSelfService = showInSelfService,
                           Caption = caption,
                           Required = required,
                           Bookmark = bookmark,
                       };
        }

        public static FieldSetting CreateForEdit(
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string bookmark)
        {
            return new FieldSetting
                       {
                           ShowInDetails = showInDetails,
                           ShowInChanges = showInChanges,
                           ShowInSelfService = showInSelfService,
                           Caption = caption,
                           Required = required,
                           Bookmark = bookmark
                       };
        }

        #endregion
    }
}