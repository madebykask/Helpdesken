namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class ImplementationModelFactory : IImplementationModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        #endregion

        #region Constructors and Destructors

        public ImplementationModelFactory(
            IConfigurableFieldModelFactory configurableFieldModelFactory, 
            ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public ImplementationModel Create(
            FindChangeResponse response, ChangeEditData editData, ImplementationEditSettings settings)
        {
            var textId = response.Change.Id.ToString(CultureInfo.InvariantCulture);
            var implementation = response.Change.Implementation;

            var status = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Status, editData.ImplementationStatuses, implementation.StatusId);

            var realStartDate = this.configurableFieldModelFactory.CreateDateTimeField(
                settings.RealStartDate, implementation.RealStartDate);

            var finishingDate = this.configurableFieldModelFactory.CreateDateTimeField(
                settings.FinishingDate, implementation.FinishingDate);

            var buildImplemented = this.configurableFieldModelFactory.CreateBooleanField(
                settings.BuildImplemented, implementation.BuildImplemented);

            var implementationPlanUsed =
                this.configurableFieldModelFactory.CreateBooleanField(
                    settings.ImplementationPlanUsed, implementation.ImplementationPlanUsed);

            var deviation = this.configurableFieldModelFactory.CreateStringField(
                settings.Deviation, implementation.Deviation);

            var recoveryPlanUsed = this.configurableFieldModelFactory.CreateBooleanField(
                settings.RecoveryPlanUsed, implementation.RecoveryPlanUsed);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles, textId, Subtopic.Implementation, response.Files);

            var logs = this.configurableFieldModelFactory.CreateLogs(
                settings.Logs, response.Change.Id, Subtopic.Implementation, response.Logs);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                editData.EmailGroups, editData.WorkingGroupsWithEmails, editData.Administrators);

            var implementationReady = this.configurableFieldModelFactory.CreateBooleanField(
                settings.ImplementationReady, response.Change.Implementation.ImplementationReady);

            return new ImplementationModel(
                response.Change.Id, 
                status, 
                realStartDate, 
                finishingDate, 
                buildImplemented, 
                implementationPlanUsed, 
                deviation, 
                recoveryPlanUsed, 
                attachedFiles, 
                logs, 
                sendToDialog, 
                implementationReady);
        }

        #endregion
    }
}