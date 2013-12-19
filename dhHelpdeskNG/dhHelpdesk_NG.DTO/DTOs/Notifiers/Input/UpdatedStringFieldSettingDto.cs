namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Input
{
    using System;

    public sealed class UpdatedStringFieldSettingDto : UpdatedFieldSettingDto
    {
        public UpdatedStringFieldSettingDto(
            bool showInDetails,
            bool showInNotifiers,
            string caption,
            bool required,
            int? minLength,
            string ldapAttribute,
            DateTime changedDateTime)
            : base(showInDetails, showInNotifiers, caption, required, ldapAttribute, changedDateTime)
        {
            this.MinLength = minLength;
        }

        public int? MinLength { get; private set; }
    }
}
