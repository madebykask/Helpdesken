namespace dhHelpdesk_NG.Service.Validators.Notifier
{
    using dhHelpdesk_NG.Service.Validators.Notifier.Settings;

    public sealed class NotifierDynamicRulesValidator : DynamicRulesValidator
    {
        public void Validate(ValidatableNotifier notifier,   FieldsValidatoSettings settings)
        {
            if (settings.Address.Required)
            {
                this.IsNotNullAndEmpty(notifier.Address, "Address");
            }

            if (settings.Address.MinLength.HasValue)
            {
                this.HasMinLength(notifier.Address, settings.Address.MinLength.Value, "Address");
            }

            if (settings.CellPhone.Required)
            {
                this.IsNotNull(notifier.CellPhone, "CellPhone");
            }

            if (settings.CellPhone.MinLength.HasValue)
            {
                this.HasMinLength(notifier.CellPhone, settings.CellPhone.MinLength.Value, "CellPhone");
            }

            if (settings.City.Required)
            {
                this.IsNotNull(notifier.City, "City");
            }

            if (settings.City.MinLength.HasValue)
            {
                this.HasMinLength(notifier.City, settings.City.MinLength.Value, "City");
            }

            if (settings.Code.Required)
            {
                this.IsNotNull(notifier.Number, "Code");
            }

            if (settings.Code.MinLength.HasValue)
            {
                this.HasMinLength(notifier.Number, settings.Code.MinLength.Value, "Code");
            }

            if (settings.Department.Required)
            {
                this.IsNotNull(notifier.DepartmentId, "Department");
            }

            if (settings.Division.Required)
            {
                this.IsNotNull(notifier.Division, "Division");
            }

            if (settings.Domain.Required)
            {
                this.IsNotNull(notifier.DomainId, "Domain");
            }

            if (settings.Email.Required)
            {
                this.IsNotNull(notifier.Email, "Email");
            }

            if (settings.Email.MinLength.HasValue)
            {
                this.HasMinLength(notifier.Email, settings.Email.MinLength.Value, "Email");
            }

            if (settings.FirstName.Required)
            {
                this.IsNotNull(notifier.FirstName, "FirstName");
            }

            if (settings.FirstName.MinLength.HasValue)
            {
                this.HasMinLength(notifier.FirstName, settings.FirstName.MinLength.Value, "FirstName");
            }

            if (settings.Group.Required)
            {
                this.IsNotNull(notifier.GroupId, "Group");
            }

            if (settings.Initials.Required)
            {
                this.IsNotNull(notifier.Initials, "Initials");
            }

            if (settings.Initials.MinLength.HasValue)
            {
                this.HasMinLength(notifier.Initials, settings.Initials.MinLength.Value, "Initials");
            }
        }
    }
}
