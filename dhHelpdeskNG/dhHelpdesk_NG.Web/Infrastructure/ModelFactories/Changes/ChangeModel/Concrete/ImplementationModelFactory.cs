namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.Edit;

    public sealed class ImplementationModelFactory : IImplementationModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        public ImplementationModelFactory(
            IConfigurableFieldModelFactory configurableFieldModelFactory,
            ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        public ImplementationModel Create(
            string temporaryId,
            ImplementationFieldEditSettings editSettings,
            ChangeEditOptions optionalData)
        {
            var implementationStatus = this.CreateImplementationStatus(null, editSettings, optionalData);
            var realStartDate = this.CreateRealStartDate(null, editSettings);
            var finishingDate = this.CreateFinishingDate(null, editSettings);
            var buildImplemented = this.CreateBuildImplemented(null, editSettings);
            var implementationPlanUsed = this.CreateImplementationPlanUsed(null, editSettings);
            var deviation = this.CreateDeviation(null, editSettings);
            var recoveryPlanUsed = this.CreateRecoveryPlanUsed(null, editSettings);

            var attachedFilesContainer = new AttachedFilesContainerModel(temporaryId, Subtopic.Implementation);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            var implementationReady = this.CreateImplementationReady(null, editSettings);

            return new ImplementationModel(
                temporaryId,
                implementationStatus,
                realStartDate,
                finishingDate,
                buildImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                attachedFilesContainer,
                sendToDialog,
                implementationReady);
        }

        public ImplementationModel Create(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings,
            ChangeEditOptions optionalData)
        {
            var id = change.Id.ToString(CultureInfo.InvariantCulture);
            var implementationStatus = this.CreateImplementationStatus(change, editSettings, optionalData);
            var realStartDate = this.CreateRealStartDate(change, editSettings);
            var finishingDate = this.CreateFinishingDate(change, editSettings);
            var buildImplemented = this.CreateBuildImplemented(change, editSettings);
            var implementationPlanUsed = this.CreateImplementationPlanUsed(change, editSettings);
            var deviation = this.CreateDeviation(change, editSettings);
            var recoveryPlanUsed = this.CreateRecoveryPlanUsed(change, editSettings);

            var attachedFilesContainer =
                new AttachedFilesContainerModel(
                    change.Id.ToString(CultureInfo.InvariantCulture),
                    Subtopic.Implementation);

            var sendToDialog = this.sendToDialogModelFactory.Create(
                optionalData.EmailGroups,
                optionalData.WorkingGroups,
                optionalData.Administrators);

            var implementationReady = this.CreateImplementationReady(change, editSettings);

            return new ImplementationModel(
                id,
                implementationStatus,
                realStartDate,
                finishingDate,
                buildImplemented,
                implementationPlanUsed,
                deviation,
                recoveryPlanUsed,
                attachedFilesContainer,
                sendToDialog,
                implementationReady);
        }

        private ConfigurableFieldModel<SelectList> CreateImplementationStatus(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings,
            ChangeEditOptions optionalData)
        {
            var selectedValue = change != null ? change.Implementation.ImplementationStatusId.ToString() : null;

            return this.configurableFieldModelFactory.CreateSelectListField(
                editSettings.State,
                optionalData.ImplementationStatuses,
                selectedValue);
        }

        private ConfigurableFieldModel<DateTime?> CreateRealStartDate(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Implementation.RealStartDate : null;
            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.RealStartDate, value);
        }

        private ConfigurableFieldModel<DateTime?> CreateFinishingDate(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Implementation.FinishingDate : null;
            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.FinishingDate, value);
        }

        private ConfigurableFieldModel<bool> CreateBuildImplemented(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Implementation.BuildImplemented : false;
            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.BuildAndTextImplemented, value);
        }

        private ConfigurableFieldModel<bool> CreateImplementationPlanUsed(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Implementation.ImplementationPlanUsed : false;
            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.ImplementationPlanUsed, value);
        }

        private ConfigurableFieldModel<string> CreateDeviation(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Implementation.ChangeDeviation : null;
            return this.configurableFieldModelFactory.CreateStringField(editSettings.Deviation, value);
        }

        private ConfigurableFieldModel<bool> CreateRecoveryPlanUsed(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Implementation.RecoveryPlanUsed : false;
            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.RecoveryPlanUsed, value);
        }

        private ConfigurableFieldModel<bool> CreateImplementationReady(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings)
        {
            var value = change != null ? change.Implementation.ImplementationReady : false;
            return this.configurableFieldModelFactory.CreateBooleanField(editSettings.ImplementationReady, value);
        } 
    }
}