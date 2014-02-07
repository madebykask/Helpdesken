namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using System;

    public sealed class UpdatedStringFieldSettingDto : UpdatedFieldSettingDto
    {
        public UpdatedStringFieldSettingDto(
            bool showInDetails,
            bool showInChanges,
            bool showInSelfService,
            string caption,
            bool required,
            string defaultValue,
            string bookmark,
            DateTime changedDateTime)
            : base(showInDetails, showInChanges, showInSelfService, caption, required, bookmark, changedDateTime)
        {
            this.DefaultValue = defaultValue;
        }

        public string DefaultValue { get; private set; }
    }
}
