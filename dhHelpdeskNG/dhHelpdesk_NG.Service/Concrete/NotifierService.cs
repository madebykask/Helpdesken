namespace dhHelpdesk_NG.Service.Concrete
{
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.Service.Validators.Notifier;
    using dhHelpdesk_NG.Service.Validators.Notifier.Settings;
    using dhHelpdesk_NG.Service.WorkflowModels.Notifiers;

    using NewNotifier = dhHelpdesk_NG.Service.WorkflowModels.Notifiers.NewNotifier;

    public sealed class NotifierService : INotifierService
    {
        private readonly INotifierDynamicRulesValidator notifierDynamicRulesValidator;

        private readonly INotifierRepository notifierRepository;

        private readonly INotifierFieldSettingRepository notifierFieldSettingRepository;

        public NotifierService(
            INotifierRepository notifierRepository,
            INotifierDynamicRulesValidator notifierDynamicRulesValidator,
            INotifierFieldSettingRepository notifierFieldSettingRepository)
        {
            this.notifierRepository = notifierRepository;
            this.notifierDynamicRulesValidator = notifierDynamicRulesValidator;
            this.notifierFieldSettingRepository = notifierFieldSettingRepository;
        }

        public void AddNotifier(NewNotifier notifier)
        {
            var newNotifier = new Validators.Notifier.Settings.NewNotifier(
                notifier.UserId,
                notifier.DomainId,
                notifier.LoginName,
                notifier.FirstName,
                notifier.Initials,
                notifier.LastName,
                notifier.DisplayName,
                notifier.Place,
                notifier.Phone,
                notifier.CellPhone,
                notifier.Email,
                notifier.Code,
                notifier.PostalAddress,
                notifier.PostalCode,
                notifier.City,
                notifier.Title,
                notifier.DepartmentId,
                notifier.Unit,
                notifier.OrganizationUnitId,
                notifier.DivisionId,
                notifier.ManagerId,
                notifier.GroupId,
                notifier.Password,
                notifier.Other,
                notifier.Ordered);

            var validationSettings = this.LoadValidationSettings(notifier.CustomerId);
            this.notifierDynamicRulesValidator.Validate(newNotifier, validationSettings);

            var newNotifierDto = new NewNotifierDto(
                notifier.CustomerId,
                notifier.UserId,
                notifier.DomainId,
                notifier.LoginName,
                notifier.FirstName,
                notifier.Initials,
                notifier.LastName,
                notifier.DisplayName,
                notifier.Place,
                notifier.Phone,
                notifier.CellPhone,
                notifier.Email,
                notifier.PostalCode,
                notifier.PostalAddress,
                notifier.PostalCode,
                notifier.City,
                notifier.Title,
                notifier.DepartmentId,
                notifier.Unit,
                notifier.OrganizationUnitId,
                notifier.DivisionId,
                notifier.ManagerId,
                notifier.GroupId,
                notifier.Password,
                notifier.Other,
                notifier.Ordered,
                notifier.IsActive,
                notifier.CreatedDate);

            this.notifierRepository.AddNotifier(newNotifierDto);
            this.notifierRepository.Commit();
        }

        public void UpdateNotifier(UpdatedNotifier notifier, int customerId)
        {
            var validatableNotifier = new Notifier(
                notifier.DomainId,
                notifier.LoginName,
                notifier.FirstName,
                notifier.Initials,
                notifier.LastName,
                notifier.DisplayName,
                notifier.Place,
                notifier.Phone,
                notifier.CellPhone,
                notifier.Email,
                notifier.Code,
                notifier.PostalAddress,
                notifier.PostalCode,
                notifier.City,
                notifier.Title,
                notifier.DepartmentId,
                notifier.Unit,
                notifier.OrganizationUnitId,
                notifier.DivisionId,
                notifier.ManagerId,
                notifier.GroupId,
                notifier.Password,
                notifier.Other,
                notifier.Ordered);

            var existingNotifierDto = this.notifierRepository.FindExistingNotifierById(notifier.Id);

            var existingNotifier = new Notifier(
                existingNotifierDto.DomainId,
                existingNotifierDto.LoginName,
                existingNotifierDto.FirstName,
                existingNotifierDto.Initials,
                existingNotifierDto.LastName,
                existingNotifierDto.DisplayName,
                existingNotifierDto.Place,
                existingNotifierDto.Phone,
                existingNotifierDto.CellPhone,
                existingNotifierDto.Email,
                existingNotifierDto.Code,
                existingNotifierDto.PostalAddress,
                existingNotifierDto.PostalCode,
                existingNotifierDto.City,
                existingNotifierDto.Title,
                existingNotifierDto.DepartmentId,
                existingNotifierDto.Unit,
                existingNotifierDto.OrganizationUnitId,
                existingNotifierDto.DivisionId,
                existingNotifierDto.ManagerId,
                existingNotifierDto.GroupId,
                existingNotifierDto.Password,
                existingNotifierDto.Other,
                existingNotifierDto.Ordered);

            var validationSettings = this.LoadValidationSettings(customerId);
            this.notifierDynamicRulesValidator.Validate(validatableNotifier, existingNotifier, validationSettings);

            var updatedNotifierDto = new UpdatedNotifierDto(
                notifier.Id,
                notifier.DomainId,
                notifier.LoginName,
                notifier.FirstName,
                notifier.Initials,
                notifier.LastName,
                notifier.DisplayName,
                notifier.Place,
                notifier.Phone,
                notifier.CellPhone,
                notifier.Email,
                notifier.Code,
                notifier.PostalAddress,
                notifier.PostalCode,
                notifier.City,
                notifier.Title,
                notifier.DepartmentId,
                notifier.Unit,
                notifier.OrganizationUnitId,
                notifier.DivisionId,
                notifier.ManagerId,
                notifier.GroupId,
                notifier.Password,
                notifier.Other,
                notifier.Ordered,
                notifier.IsActive,
                notifier.ChangedDate);

            this.notifierRepository.UpdateNotifier(updatedNotifierDto);
            this.notifierRepository.Commit();
        }

        private FieldValidationSettings LoadValidationSettings(int customerId)
        {
            var vs = this.notifierFieldSettingRepository.FindFieldDisplayRulesByCustomerId(customerId);

            return
                new FieldValidationSettings(
                    new FieldValidationSetting(!vs.UserId.Show, vs.UserId.Required, vs.UserId.MinLength),
                    new FieldValidationSetting(!vs.Domain.Show, vs.Domain.Required, vs.Domain.MinLength),
                    new FieldValidationSetting(!vs.LoginName.Show, vs.LoginName.Required, vs.LoginName.MinLength),
                    new FieldValidationSetting(!vs.FirstName.Show, vs.FirstName.Required, vs.FirstName.MinLength),
                    new FieldValidationSetting(!vs.Initials.Show, vs.Initials.Required, vs.Initials.MinLength),
                    new FieldValidationSetting(!vs.LastName.Show, vs.LastName.Required, vs.LastName.MinLength),
                    new FieldValidationSetting(!vs.DisplayName.Show, vs.DisplayName.Required, vs.DisplayName.MinLength),
                    new FieldValidationSetting(!vs.Place.Show, vs.Place.Required, vs.Place.MinLength),
                    new FieldValidationSetting(!vs.Phone.Show, vs.Phone.Required, vs.Phone.MinLength),
                    new FieldValidationSetting(!vs.CellPhone.Show, vs.CellPhone.Required, vs.CellPhone.MinLength),
                    new FieldValidationSetting(!vs.Email.Show, vs.Email.Required, vs.Email.MinLength),
                    new FieldValidationSetting(!vs.Code.Show, vs.Code.Required, vs.Code.MinLength),
                    new FieldValidationSetting(
                        !vs.PostalAddress.Show, vs.PostalAddress.Required, vs.PostalAddress.MinLength),
                    new FieldValidationSetting(!vs.PostalCode.Show, vs.PostalCode.Required, vs.PostalCode.MinLength),
                    new FieldValidationSetting(!vs.City.Show, vs.City.Required, vs.City.MinLength),
                    new FieldValidationSetting(!vs.Title.Show, vs.Title.Required, vs.Title.MinLength),
                    new FieldValidationSetting(!vs.Department.Show, vs.Department.Required, vs.Department.MinLength),
                    new FieldValidationSetting(!vs.Unit.Show, vs.Unit.Required, vs.Unit.MinLength),
                    new FieldValidationSetting(
                        !vs.OrganizationUnit.Show, vs.OrganizationUnit.Required, vs.OrganizationUnit.MinLength),
                    new FieldValidationSetting(!vs.Division.Show, vs.Division.Required, vs.Division.MinLength),
                    new FieldValidationSetting(!vs.Manager.Show, vs.Manager.Required, vs.Manager.MinLength),
                    new FieldValidationSetting(!vs.Group.Show, vs.Group.Required, vs.Group.MinLength),
                    new FieldValidationSetting(!vs.Password.Show, vs.Password.Required, vs.Password.MinLength),
                    new FieldValidationSetting(!vs.Other.Show, vs.Other.Required, vs.Other.MinLength),
                    new FieldValidationSetting(!vs.Ordered.Show, vs.Ordered.Required, vs.Ordered.MinLength));
        }
    }
}
