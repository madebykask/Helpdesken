namespace DH.Helpdesk.Dal.Mappers.Changes.BusinessModelToEntity
{
    using System;

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
            MapOrdererSettings(businessModel.Orderer, entity, businessModel.ChangedDateAndTime);
            MapGeneralSettings(businessModel.General, entity, businessModel.ChangedDateAndTime);
            MapRegistrationSettings(businessModel.Registration, entity, businessModel.ChangedDateAndTime);
            MapAnalyzeSettings(businessModel.Analyze, entity, businessModel.ChangedDateAndTime);
            MapImplementationSettings(businessModel.Implementation, entity, businessModel.ChangedDateAndTime);
            MapEvaluationSettings(businessModel.Evaluation, entity, businessModel.ChangedDateAndTime);
            MapLogSettings(businessModel.Log, entity, businessModel.ChangedDateAndTime);
        }

        private static void MapOrdererSettings(
            OrdererFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings, DateTime changedDateAndTime)
        {
            var id = existingSettings.FindByName(OrdererField.Id);
            MapFieldSetting(updatedSettings.Id, id, changedDateAndTime);

            var name = existingSettings.FindByName(OrdererField.Name);
            MapFieldSetting(updatedSettings.Name, name, changedDateAndTime);

            var phone = existingSettings.FindByName(OrdererField.Phone);
            MapFieldSetting(updatedSettings.Phone, phone, changedDateAndTime);

            var cellPhone = existingSettings.FindByName(OrdererField.CellPhone);
            MapFieldSetting(updatedSettings.CellPhone, cellPhone, changedDateAndTime);

            var email = existingSettings.FindByName(OrdererField.Email);
            MapFieldSetting(updatedSettings.Email, email, changedDateAndTime);

            var department = existingSettings.FindByName(OrdererField.Department);
            MapFieldSetting(updatedSettings.Department, department, changedDateAndTime);
        }

        private static void MapGeneralSettings(
            GeneralFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            DateTime changedDateAndTime)
        {
            var priority = existingSettings.FindByName(GeneralField.Priority);
            MapFieldSetting(updatedSettings.Priority, priority, changedDateAndTime);

            var title = existingSettings.FindByName(GeneralField.Title);
            MapFieldSetting(updatedSettings.Title, title, changedDateAndTime);

            var state = existingSettings.FindByName(GeneralField.Status);
            MapFieldSetting(updatedSettings.Status, state, changedDateAndTime);

            var system = existingSettings.FindByName(GeneralField.System);
            MapFieldSetting(updatedSettings.System, system, changedDateAndTime);

            var @object = existingSettings.FindByName(GeneralField.Object);
            MapFieldSetting(updatedSettings.Object, @object, changedDateAndTime);

            var inventory = existingSettings.FindByName(GeneralField.Inventory);
            MapFieldSetting(updatedSettings.Inventory, inventory, changedDateAndTime);

            var workingGroup = existingSettings.FindByName(GeneralField.WorkingGroup);
            MapFieldSetting(updatedSettings.WorkingGroup, workingGroup, changedDateAndTime);

            var administrator = existingSettings.FindByName(GeneralField.Administrator);
            MapFieldSetting(updatedSettings.Administrator, administrator, changedDateAndTime);

            var finishingDate = existingSettings.FindByName(GeneralField.FinishingDate);
            MapFieldSetting(updatedSettings.FinishingDate, finishingDate, changedDateAndTime);

            var rss = existingSettings.FindByName(GeneralField.Rss);
            MapFieldSetting(updatedSettings.Rss, rss, changedDateAndTime);
        }

        private static void MapRegistrationSettings(
            RegistrationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            DateTime changedDateAndTime)
        {
            var name = existingSettings.FindByName(RegistrationField.Name);
            MapFieldSetting(updatedSettings.Name, name, changedDateAndTime);

            var phone = existingSettings.FindByName(RegistrationField.Phone);
            MapFieldSetting(updatedSettings.Phone, phone, changedDateAndTime);

            var email = existingSettings.FindByName(RegistrationField.Email);
            MapFieldSetting(updatedSettings.Email, email, changedDateAndTime);

            var company = existingSettings.FindByName(RegistrationField.Company);
            MapFieldSetting(updatedSettings.Company, company, changedDateAndTime);

            var owner = existingSettings.FindByName(RegistrationField.Owner);
            MapFieldSetting(updatedSettings.Owner, owner, changedDateAndTime);

            var affectedProcesses = existingSettings.FindByName(RegistrationField.AffectedProcesses);
            MapFieldSetting(updatedSettings.AffectedProcesses, affectedProcesses, changedDateAndTime);

            var affectedDepartments = existingSettings.FindByName(RegistrationField.AffectedDepartments);
            MapFieldSetting(updatedSettings.AffectedDepartments, affectedDepartments, changedDateAndTime);

            var description = existingSettings.FindByName(RegistrationField.Description);
            MapTextFieldSetting(description, updatedSettings.Description, changedDateAndTime);

            var businessBenefits = existingSettings.FindByName(RegistrationField.BusinessBenefits);
            MapTextFieldSetting(businessBenefits, updatedSettings.BusinessBenefits, changedDateAndTime);

            var consequence = existingSettings.FindByName(RegistrationField.Consequence);
            MapTextFieldSetting(consequence, updatedSettings.Consequence, changedDateAndTime);

            var impact = existingSettings.FindByName(RegistrationField.Impact);
            MapFieldSetting(updatedSettings.Impact, impact, changedDateAndTime);

            var desiredDate = existingSettings.FindByName(RegistrationField.DesiredDate);
            MapFieldSetting(updatedSettings.DesiredDate, desiredDate, changedDateAndTime);

            var verified = existingSettings.FindByName(RegistrationField.Verified);
            MapFieldSetting(updatedSettings.Verified, verified, changedDateAndTime);

            var attachedFiles = existingSettings.FindByName(RegistrationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles, changedDateAndTime);

            var approval = existingSettings.FindByName(RegistrationField.Approval);
            MapFieldSetting(updatedSettings.Approval, approval, changedDateAndTime);

            var rejectExplanation = existingSettings.FindByName(RegistrationField.RejectExplanation);
            MapFieldSetting(updatedSettings.RejectExplanation, rejectExplanation, changedDateAndTime);
        }

        private static void MapAnalyzeSettings(
            AnalyzeFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            DateTime changedDateAndTime)
        {
            var category = existingSettings.FindByName(AnalyzeField.Category);
            MapFieldSetting(updatedSettings.Category, category, changedDateAndTime);

            var priority = existingSettings.FindByName(AnalyzeField.Priority);
            MapFieldSetting(updatedSettings.Priority, priority, changedDateAndTime);

            var responsible = existingSettings.FindByName(AnalyzeField.Responsible);
            MapFieldSetting(updatedSettings.Responsible, responsible, changedDateAndTime);

            var solution = existingSettings.FindByName(AnalyzeField.Solution);
            MapTextFieldSetting(solution, updatedSettings.Solution, changedDateAndTime);

            var cost = existingSettings.FindByName(AnalyzeField.Cost);
            MapFieldSetting(updatedSettings.Cost, cost, changedDateAndTime);

            var yearlyCost = existingSettings.FindByName(AnalyzeField.YearlyCost);
            MapFieldSetting(updatedSettings.YearlyCost, yearlyCost, changedDateAndTime);

            var estimatedTimeInHours = existingSettings.FindByName(AnalyzeField.EstimatedTimeInHours);
            MapFieldSetting(updatedSettings.EstimatedTimeInHours, estimatedTimeInHours, changedDateAndTime);

            var risk = existingSettings.FindByName(AnalyzeField.Risk);
            MapTextFieldSetting(risk, updatedSettings.Risk, changedDateAndTime);

            var startDate = existingSettings.FindByName(AnalyzeField.StartDate);
            MapFieldSetting(updatedSettings.StartDate, startDate, changedDateAndTime);

            var finishDate = existingSettings.FindByName(AnalyzeField.FinishDate);
            MapFieldSetting(updatedSettings.FinishDate, finishDate, changedDateAndTime);

            var hasImplementationPlan = existingSettings.FindByName(AnalyzeField.HasImplementationPlan);
            MapFieldSetting(updatedSettings.HasImplementationPlan, hasImplementationPlan, changedDateAndTime);

            var hasRecoveryPlan = existingSettings.FindByName(AnalyzeField.HasRecoveryPlan);
            MapFieldSetting(updatedSettings.HasRecoveryPlan, hasRecoveryPlan, changedDateAndTime);

            var attachedFiles = existingSettings.FindByName(AnalyzeField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles, changedDateAndTime);

            var logs = existingSettings.FindByName(AnalyzeField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs, changedDateAndTime);

            var approval = existingSettings.FindByName(AnalyzeField.Approval);
            MapFieldSetting(updatedSettings.Approval, approval, changedDateAndTime);

            var rejectExplanation = existingSettings.FindByName(AnalyzeField.RejectExplanation);
            MapTextFieldSetting(rejectExplanation, updatedSettings.RejectExplanation, changedDateAndTime);
        }

        private static void MapImplementationSettings(
            ImplementationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            DateTime changedDateAndTime)
        {
            var status = existingSettings.FindByName(ImplementationField.ImplementationStatus);
            MapFieldSetting(updatedSettings.Status, status, changedDateAndTime);

            var realStartDate = existingSettings.FindByName(ImplementationField.RealStartDate);
            MapFieldSetting(updatedSettings.RealStartDate, realStartDate, changedDateAndTime);

            var buildImplemented = existingSettings.FindByName(ImplementationField.BuildImplemented);
            MapFieldSetting(updatedSettings.BuildImplemented, buildImplemented, changedDateAndTime);

            var implementationPlanUsed = existingSettings.FindByName(ImplementationField.ImplementationPlanUsed);
            MapFieldSetting(updatedSettings.ImplementationPlanUsed, implementationPlanUsed, changedDateAndTime);

            var deviation = existingSettings.FindByName(ImplementationField.Deviation);
            MapTextFieldSetting(deviation, updatedSettings.Deviation, changedDateAndTime);

            var recoveryPlanUsed = existingSettings.FindByName(ImplementationField.RecoveryPlanUsed);
            MapFieldSetting(updatedSettings.RecoveryPlanUsed, recoveryPlanUsed, changedDateAndTime);

            var finishingDate = existingSettings.FindByName(ImplementationField.FinishingDate);
            MapFieldSetting(updatedSettings.FinishingDate, finishingDate, changedDateAndTime);

            var attachedFiles = existingSettings.FindByName(ImplementationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles, changedDateAndTime);

            var logs = existingSettings.FindByName(ImplementationField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs, changedDateAndTime);

            var implementationReady = existingSettings.FindByName(ImplementationField.ImplementationReady);
            MapFieldSetting(updatedSettings.ImplementationReady, implementationReady, changedDateAndTime);
        }

        private static void MapEvaluationSettings(
            EvaluationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            DateTime changedDateAndTime)
        {
            var changeEvaluation = existingSettings.FindByName(EvaluationField.ChangeEvaluation);
            MapTextFieldSetting(changeEvaluation, updatedSettings.Evaluation, changedDateAndTime);

            var attachedFiles = existingSettings.FindByName(EvaluationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles, changedDateAndTime);

            var logs = existingSettings.FindByName(EvaluationField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs, changedDateAndTime);

            var evaluationReady = existingSettings.FindByName(EvaluationField.EvaluationReady);
            MapFieldSetting(updatedSettings.EvaluationReady, evaluationReady, changedDateAndTime);
        }

        private static void MapLogSettings(
            LogFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            DateTime changedDateAndTime)
        {
            var logs = existingSettings.FindByName(LogField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs, changedDateAndTime);
        }

        private static void MapFieldSetting(
            FieldSetting updatedSetting,
            ChangeFieldSettingsEntity fieldSetting,
            DateTime changedDateAndTime)
        {
            fieldSetting.Bookmark = updatedSetting.Bookmark;
            fieldSetting.ChangedDate = changedDateAndTime;
            fieldSetting.Required = updatedSetting.Required.ToInt();
            fieldSetting.Show = updatedSetting.ShowInDetails.ToInt();
            fieldSetting.ShowExternal = updatedSetting.ShowInSelfService.ToInt();
            fieldSetting.ShowInList = updatedSetting.ShowInChanges.ToInt();
        }

        private static void MapTextFieldSetting(
            ChangeFieldSettingsEntity fieldSetting,
            TextFieldSetting updatedSetting,
            DateTime changedDateAndTime)
        {
            fieldSetting.Bookmark = updatedSetting.Bookmark;
            fieldSetting.ChangedDate = changedDateAndTime;
            fieldSetting.InitialValue = updatedSetting.DefaultValue;
            fieldSetting.Required = updatedSetting.Required.ToInt();
            fieldSetting.Show = updatedSetting.ShowInDetails.ToInt();
            fieldSetting.ShowExternal = updatedSetting.ShowInSelfService.ToInt();
            fieldSetting.ShowInList = updatedSetting.ShowInChanges.ToInt();
        }
    }
}
