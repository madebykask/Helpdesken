namespace DH.Helpdesk.Dal.Mappers.Changes.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Enums.Changes;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeFieldSettingsToChangeFieldSettingsEntityMapper :
        IBusinessModelToEntityMapper<ChangeFieldSettings, NamedObjectCollection<ChangeFieldSettingsEntity>>
    {
        public void Map(ChangeFieldSettings businessModel, NamedObjectCollection<ChangeFieldSettingsEntity> entity)
        {
            MapOrdererSettings(businessModel.Orderer, entity);
            MapGeneralSettings(businessModel.General, entity);
            MapRegistrationSettings(businessModel.Registration, entity);
            MapAnalyzeSettings(businessModel.Analyze, entity);
            MapImplementationSettings(businessModel.Implementation, entity);
            MapEvaluationSettings(businessModel.Evaluation, entity);
            MapLogSettings(businessModel.Log, entity);
        }

        private static void MapOrdererSettings(
            OrdererFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings)
        {
            var id = existingSettings.FindByName(OrdererField.Id);
            MapFieldSetting(updatedSettings.Id, id);

            var name = existingSettings.FindByName(OrdererField.Name);
            MapFieldSetting(updatedSettings.Name, name);

            var phone = existingSettings.FindByName(OrdererField.Phone);
            MapFieldSetting(updatedSettings.Phone, phone);

            var cellPhone = existingSettings.FindByName(OrdererField.CellPhone);
            MapFieldSetting(updatedSettings.CellPhone, cellPhone);

            var email = existingSettings.FindByName(OrdererField.Email);
            MapFieldSetting(updatedSettings.Email, email);

            var department = existingSettings.FindByName(OrdererField.Department);
            MapFieldSetting(updatedSettings.Department, department);
        }

        private static void MapGeneralSettings(
            GeneralFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings)
        {
            var priority = existingSettings.FindByName(GeneralField.Priority);
            MapFieldSetting(updatedSettings.Priority, priority);

            var title = existingSettings.FindByName(GeneralField.Title);
            MapFieldSetting(updatedSettings.Title, title);

            var state = existingSettings.FindByName(GeneralField.Status);
            MapFieldSetting(updatedSettings.Status, state);

            var system = existingSettings.FindByName(GeneralField.System);
            MapFieldSetting(updatedSettings.System, system);

            var @object = existingSettings.FindByName(GeneralField.Object);
            MapFieldSetting(updatedSettings.Object, @object);

            var inventory = existingSettings.FindByName(GeneralField.Inventory);
            MapFieldSetting(updatedSettings.Inventory, inventory);

            var workingGroup = existingSettings.FindByName(GeneralField.WorkingGroup);
            MapFieldSetting(updatedSettings.WorkingGroup, workingGroup);

            var administrator = existingSettings.FindByName(GeneralField.Administrator);
            MapFieldSetting(updatedSettings.Administrator, administrator);

            var finishingDate = existingSettings.FindByName(GeneralField.FinishingDate);
            MapFieldSetting(updatedSettings.FinishingDate, finishingDate);

            var rss = existingSettings.FindByName(GeneralField.Rss);
            MapFieldSetting(updatedSettings.Rss, rss);
        }

        private static void MapRegistrationSettings(
            RegistrationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings)
        {
            var owner = existingSettings.FindByName(RegistrationField.Owner);
            MapFieldSetting(updatedSettings.Owner, owner);

            var affectedProcesses = existingSettings.FindByName(RegistrationField.AffectedProcesses);
            MapFieldSetting(updatedSettings.AffectedProcesses, affectedProcesses);

            var affectedDepartments = existingSettings.FindByName(RegistrationField.AffectedDepartments);
            MapFieldSetting(updatedSettings.AffectedDepartments, affectedDepartments);

            var description = existingSettings.FindByName(RegistrationField.Description);
            MapTextFieldSetting(description, updatedSettings.Description);

            var businessBenefits = existingSettings.FindByName(RegistrationField.BusinessBenefits);
            MapTextFieldSetting(businessBenefits, updatedSettings.BusinessBenefits);

            var consequence = existingSettings.FindByName(RegistrationField.Consequence);
            MapTextFieldSetting(consequence, updatedSettings.Consequence);

            var impact = existingSettings.FindByName(RegistrationField.Impact);
            MapFieldSetting(updatedSettings.Impact, impact);

            var desiredDate = existingSettings.FindByName(RegistrationField.DesiredDate);
            MapFieldSetting(updatedSettings.DesiredDate, desiredDate);

            var verified = existingSettings.FindByName(RegistrationField.Verified);
            MapFieldSetting(updatedSettings.Verified, verified);

            var attachedFiles = existingSettings.FindByName(RegistrationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles);

            var approval = existingSettings.FindByName(RegistrationField.Approval);
            MapFieldSetting(updatedSettings.Approval, approval);

            var rejectExplanation = existingSettings.FindByName(RegistrationField.RejectExplanation);
            MapFieldSetting(updatedSettings.RejectExplanation, rejectExplanation);
        }

        private static void MapAnalyzeSettings(
            AnalyzeFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings)
        {
            var category = existingSettings.FindByName(AnalyzeField.Category);
            MapFieldSetting(updatedSettings.Category, category);

            var priority = existingSettings.FindByName(AnalyzeField.Priority);
            MapFieldSetting(updatedSettings.Priority, priority);

            var responsible = existingSettings.FindByName(AnalyzeField.Responsible);
            MapFieldSetting(updatedSettings.Responsible, responsible);

            var solution = existingSettings.FindByName(AnalyzeField.Solution);
            MapTextFieldSetting(solution, updatedSettings.Solution);

            var cost = existingSettings.FindByName(AnalyzeField.Cost);
            MapFieldSetting(updatedSettings.Cost, cost);

            var yearlyCost = existingSettings.FindByName(AnalyzeField.YearlyCost);
            MapFieldSetting(updatedSettings.YearlyCost, yearlyCost);

            var estimatedTimeInHours = existingSettings.FindByName(AnalyzeField.EstimatedTimeInHours);
            MapFieldSetting(updatedSettings.EstimatedTimeInHours, estimatedTimeInHours);

            var risk = existingSettings.FindByName(AnalyzeField.Risk);
            MapTextFieldSetting(risk, updatedSettings.Risk);

            var startDate = existingSettings.FindByName(AnalyzeField.StartDate);
            MapFieldSetting(updatedSettings.StartDate, startDate);

            var finishDate = existingSettings.FindByName(AnalyzeField.FinishDate);
            MapFieldSetting(updatedSettings.FinishDate, finishDate);

            var hasImplementationPlan = existingSettings.FindByName(AnalyzeField.HasImplementationPlan);
            MapFieldSetting(updatedSettings.HasImplementationPlan, hasImplementationPlan);

            var hasRecoveryPlan = existingSettings.FindByName(AnalyzeField.HasRecoveryPlan);
            MapFieldSetting(updatedSettings.HasRecoveryPlan, hasRecoveryPlan);

            var attachedFiles = existingSettings.FindByName(AnalyzeField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles);

            var logs = existingSettings.FindByName(AnalyzeField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs);

            var approval = existingSettings.FindByName(AnalyzeField.Approval);
            MapFieldSetting(updatedSettings.Approval, approval);

            var rejectExplanation = existingSettings.FindByName(AnalyzeField.RejectExplanation);
            MapTextFieldSetting(rejectExplanation, updatedSettings.RejectExplanation);
        }

        private static void MapImplementationSettings(
            ImplementationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings)
        {
            var status = existingSettings.FindByName(ImplementationField.Status);
            MapFieldSetting(updatedSettings.Status, status);

            var realStartDate = existingSettings.FindByName(ImplementationField.RealStartDate);
            MapFieldSetting(updatedSettings.RealStartDate, realStartDate);

            var buildImplemented = existingSettings.FindByName(ImplementationField.BuildImplemented);
            MapFieldSetting(updatedSettings.BuildImplemented, buildImplemented);

            var implementationPlanUsed = existingSettings.FindByName(ImplementationField.ImplementationPlanUsed);
            MapFieldSetting(updatedSettings.ImplementationPlanUsed, implementationPlanUsed);

            var deviation = existingSettings.FindByName(ImplementationField.Deviation);
            MapTextFieldSetting(deviation, updatedSettings.Deviation);

            var recoveryPlanUsed = existingSettings.FindByName(ImplementationField.RecoveryPlanUsed);
            MapFieldSetting(updatedSettings.RecoveryPlanUsed, recoveryPlanUsed);

            var finishingDate = existingSettings.FindByName(ImplementationField.FinishingDate);
            MapFieldSetting(updatedSettings.FinishingDate, finishingDate);

            var attachedFiles = existingSettings.FindByName(ImplementationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles);

            var logs = existingSettings.FindByName(ImplementationField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs);

            var implementationReady = existingSettings.FindByName(ImplementationField.ImplementationReady);
            MapFieldSetting(updatedSettings.ImplementationReady, implementationReady);
        }

        private static void MapEvaluationSettings(
            EvaluationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings)
        {
            var changeEvaluation = existingSettings.FindByName(EvaluationField.ChangeEvaluation);
            MapTextFieldSetting(changeEvaluation, updatedSettings.Evaluation);

            var attachedFiles = existingSettings.FindByName(EvaluationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles);

            var logs = existingSettings.FindByName(EvaluationField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs);

            var evaluationReady = existingSettings.FindByName(EvaluationField.EvaluationReady);
            MapFieldSetting(updatedSettings.EvaluationReady, evaluationReady);
        }

        private static void MapLogSettings(
            LogFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings)
        {
            var logs = existingSettings.FindByName(LogField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs);
        }

        private static void MapFieldSetting(
            FieldSetting updatedSetting,
            ChangeFieldSettingsEntity fieldSetting)
        {
            fieldSetting.Bookmark = updatedSetting.Bookmark;
            fieldSetting.ChangedDate = updatedSetting.ChangedDateAndTime.Value;
            fieldSetting.Required = updatedSetting.Required.ToInt();
            fieldSetting.Show = updatedSetting.ShowInDetails.ToInt();
            fieldSetting.ShowExternal = updatedSetting.ShowInSelfService.ToInt();
            fieldSetting.ShowInList = updatedSetting.ShowInChanges.ToInt();
        }

        private static void MapTextFieldSetting(
            ChangeFieldSettingsEntity fieldSetting,
            TextFieldSetting updatedSetting)
        {
            fieldSetting.Bookmark = updatedSetting.Bookmark;
            fieldSetting.ChangedDate = updatedSetting.ChangedDateAndTime.Value;
            fieldSetting.InitialValue = updatedSetting.DefaultValue;
            fieldSetting.Required = updatedSetting.Required.ToInt();
            fieldSetting.Show = updatedSetting.ShowInDetails.ToInt();
            fieldSetting.ShowExternal = updatedSetting.ShowInSelfService.ToInt();
            fieldSetting.ShowInList = updatedSetting.ShowInChanges.ToInt();
        }
    }
}
