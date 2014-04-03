namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistrationViewModel
    {
        #region Constructors and Destructors

        public RegistrationViewModel()
        {
        }

        public RegistrationViewModel(
            ConfigurableFieldModel<SelectList> owners,
            ConfigurableFieldModel<MultiSelectList> affectedDepartments,
            ConfigurableFieldModel<MultiSelectList> affectedProcesses,
            ConfigurableFieldModel<SelectList> approvalResults,
            RegistrationModel registration)
        {
            this.Owners = owners;
            this.AffectedDepartments = affectedDepartments;
            this.AffectedProcesses = affectedProcesses;
            this.ApprovalResults = approvalResults;
            this.Registration = registration;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<MultiSelectList> AffectedDepartments { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<MultiSelectList> AffectedProcesses { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ApprovalResults { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Owners { get; private set; }

        [NotNull]
        public RegistrationModel Registration { get; set; }

        #endregion
    }
}