namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes.ApprovalResult;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewRegistrationModelFactory : INewRegistrationModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public NewRegistrationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public RegistrationModel Create(string temporaryId, ChangeEditData editData, RegistrationEditSettings settings)
        {
            var owner = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Owner, editData.Owners, (int?)null);

            var affectedProcesses =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedProcesses, editData.AffectedProcesses, (List<int>)null);

            var affectedDepartments =
                this.configurableFieldModelFactory.CreateMultiSelectListField(
                    settings.AffectedDepartments, editData.AffectedDepartments, (List<int>)null);

            var description = this.configurableFieldModelFactory.CreateStringField(settings.Description, null);
            var businessBenefits = this.configurableFieldModelFactory.CreateStringField(settings.BusinessBenefits, null);
            var consequence = this.configurableFieldModelFactory.CreateStringField(settings.Consequence, null);
            var impact = this.configurableFieldModelFactory.CreateStringField(settings.Impact, null);

            var desiredDateAndTime = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.DesiredDate, null);

            var verified = this.configurableFieldModelFactory.CreateBooleanField(settings.Verified, false);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles, temporaryId, new List<File>(0));

            var approvalList = CreateApprovalList();
            var approval = this.configurableFieldModelFactory.CreateSelectListField(settings.Approval, approvalList);

            var rejectExplanation = this.configurableFieldModelFactory.CreateStringField(
                settings.RejectExplanation, null);

            return new RegistrationModel(
                temporaryId,
                owner,
                affectedProcesses,
                affectedDepartments,
                description,
                businessBenefits,
                consequence,
                impact,
                desiredDateAndTime,
                verified,
                attachedFiles,
                approval,
                null,
                null,
                rejectExplanation);
        }

        private static SelectList CreateApprovalList()
        {
            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approve", Enums.TranslationSource.TextTranslation);
            approveItem.Value = RegistrationApprovalResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = RegistrationApprovalResult.Rejected.ToString();

            var approvalItems = new List<object> { approveItem, rejectItem };
            return new SelectList(approvalItems, "Value", "Text");
        }
    }
}