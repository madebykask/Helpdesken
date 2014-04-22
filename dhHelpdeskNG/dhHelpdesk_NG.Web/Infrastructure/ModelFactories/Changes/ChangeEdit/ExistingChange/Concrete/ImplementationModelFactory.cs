namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class ImplementationModelFactory : IImplementationModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public ImplementationModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public ImplementationModel Create(FindChangeResponse response)
        {
            var settings = response.EditSettings.Implementation;
            var fields = response.EditData.Change.Implementation;
            var options = response.EditOptions;

            var textId = response.EditData.Change.Id.ToString(CultureInfo.InvariantCulture);

            var statuses = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Status,
                options.ImplementationStatuses,
                fields.StatusId.ToString());

            var realStartDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.RealStartDate,
                fields.RealStartDate);

            var finishingDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.FinishingDate,
                fields.FinishingDate);

            var buildImplemented = this.configurableFieldModelFactory.CreateBooleanField(
                settings.BuildImplemented,
                fields.BuildImplemented);

            var implementationPlanUsed =
                this.configurableFieldModelFactory.CreateBooleanField(
                    settings.ImplementationPlanUsed,
                    fields.ImplementationPlanUsed);

            var changeDeviation = this.configurableFieldModelFactory.CreateStringField(
                settings.Deviation,
                fields.Deviation);

            var recoveryPlanUsed = this.configurableFieldModelFactory.CreateBooleanField(
                settings.RecoveryPlanUsed,
                fields.RecoveryPlanUsed);

            var attachedFiles = this.configurableFieldModelFactory.CreateAttachedFiles(
                settings.AttachedFiles,
                textId,
                Subtopic.Implementation,
                response.EditData.Files.Where(f => f.Subtopic == Subtopic.Implementation).Select(f => f.Name).ToList());

            var logs = this.configurableFieldModelFactory.CreateLogs(
                settings.Logs,
                response.EditData.Change.Id,
                Subtopic.Implementation,
                response.EditData.Logs.Where(l => l.Subtopic == Subtopic.Implementation).ToList(),
                options.EmailGroups,
                options.WorkingGroupsWithEmails,
                options.Administrators);

            var implementationReady = this.configurableFieldModelFactory.CreateBooleanField(
                settings.ImplementationReady,
                fields.ImplementationReady);

            return new ImplementationModel(
                response.EditData.Change.Id,
                statuses,
                realStartDate,
                finishingDate,
                buildImplemented,
                implementationPlanUsed,
                changeDeviation,
                recoveryPlanUsed,
                attachedFiles,
                logs,
                implementationReady);
        }

        #endregion
    }
}