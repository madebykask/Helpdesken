namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class RegistrationModelFactory : IRegistrationModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public RegistrationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public RegistrationModel Create(
            FindChangeResponse response, ChangeEditData editData, RegistrationEditSettings settings)
        {
            var textId = response.Change.Id.ToString(CultureInfo.InvariantCulture);
            var registration = response.Change.Registration;

            var owner = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Owner, editData.Owners, registration.OwnerId);

            var affectedProcesses =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedProcesses,
                    editData.AffectedProcesses,
                    response.AffectedProcessIds.Cast<object>().ToList());

            var affectedDepartments =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedDepartments,
                    editData.Departments,
                    response.AffectedDepartmentIds.Cast<object>().ToList());

            var description = this.configurableFieldModelFactory.CreateStringField(
                settings.Description, registration.Description);

            var businessBenefits = this.configurableFieldModelFactory.CreateStringField(
                settings.BusinessBenefits, response.Change.Registration.BusinessBenefits);

            var consequence = this.configurableFieldModelFactory.CreateStringField(
                settings.Consequence, response.Change.Registration.Consequence);

            var impact = this.configurableFieldModelFactory.CreateStringField(
                settings.Impact, response.Change.Registration.Impact);

            var desiredDate = this.configurableFieldModelFactory.CreateDateTimeField(
                settings.DesiredDate, response.Change.Registration.DesiredDate);

            var verified = this.configurableFieldModelFactory.CreateBooleanField(
                settings.Verified, response.Change.Registration.Verified);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles, textId, Subtopic.Registration, response.Files);

            var approvalItems = CreateApprovalItems();

            var approval = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Approval, approvalItems, registration.Approval);

            var rejectExplanation = this.configurableFieldModelFactory.CreateStringField(
                settings.RejectExplanation, response.Change.Registration.RejectExplanation);

            return new RegistrationModel(
                textId,
                owner,
                affectedProcesses,
                affectedDepartments,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDate,
                verified,
                attachedFiles,
                approval,
                response.Change.Registration.ApprovedDateAndTime,
                response.Change.Registration.ApprovedByUser,
                rejectExplanation);
        }

        private static List<SelectListItem> CreateApprovalItems()
        {
            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approve", Enums.TranslationSource.TextTranslation);
            approveItem.Value = AnalyzeApprovalResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = AnalyzeApprovalResult.Rejected.ToString();

            return new List<SelectListItem> { approveItem, rejectItem };
        }
    }
}