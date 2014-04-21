namespace DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit
{
    using System;

    public sealed class TextFieldSetting : FieldSetting
    {
        #region Public Properties

        public string DefaultValue { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static TextFieldSetting CreateForEdit(
            bool showInDetails, 
            bool showInChanges, 
            bool showInSelfService, 
            string caption, 
            bool required, 
            string defaultValue, 
            string bookmark)
        {
            return new TextFieldSetting
                       {
                           ShowInDetails = showInDetails, 
                           ShowInChanges = showInChanges, 
                           ShowInSelfService = showInSelfService, 
                           Caption = caption, 
                           Required = required, 
                           DefaultValue = defaultValue, 
                           Bookmark = bookmark
                       };
        }

        public static TextFieldSetting CreateUpdated(
            bool showInDetails, 
            bool showInChanges, 
            bool showInSelfService, 
            string caption, 
            bool required, 
            string defaultValue, 
            string bookmark)
        {
            return new TextFieldSetting
                       {
                           ShowInDetails = showInDetails, 
                           ShowInChanges = showInChanges, 
                           ShowInSelfService = showInSelfService, 
                           Caption = caption, 
                           Required = required, 
                           DefaultValue = defaultValue, 
                           Bookmark = bookmark, 
                       };
        }

        #endregion
    }
}