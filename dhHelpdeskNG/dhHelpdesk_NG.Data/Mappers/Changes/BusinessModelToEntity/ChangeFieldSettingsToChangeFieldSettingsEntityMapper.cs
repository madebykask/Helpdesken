namespace DH.Helpdesk.Dal.Mappers.Changes.BusinessModelToEntity
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Enums.Changes;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeFieldSettingsToChangeFieldSettingsEntityMapper :
        IBusinessModelToEntityMapper<ChangeFieldSettings, NamedObjectCollection<ChangeFieldSettingsEntity>>
    {
        public void Map(ChangeFieldSettings businessModel, NamedObjectCollection<ChangeFieldSettingsEntity> entity)
        {
            MapOrdererSettings(businessModel.Orderer, entity, businessModel.LanguageId.Value, businessModel.ChangedDateAndTime);
            MapGeneralSettings(businessModel.General, entity, businessModel.LanguageId.Value, businessModel.ChangedDateAndTime);
            MapRegistrationSettings(businessModel.Registration, entity, businessModel.LanguageId.Value, businessModel.ChangedDateAndTime);
            MapAnalyzeSettings(businessModel.Analyze, entity, businessModel.LanguageId.Value, businessModel.ChangedDateAndTime);
            MapImplementationSettings(businessModel.Implementation, entity, businessModel.LanguageId.Value, businessModel.ChangedDateAndTime);
            MapEvaluationSettings(businessModel.Evaluation, entity, businessModel.LanguageId.Value, businessModel.ChangedDateAndTime);
            MapLogSettings(businessModel.Log, entity, businessModel.LanguageId.Value, businessModel.ChangedDateAndTime);
        }

        private static void MapOrdererSettings(
            OrdererFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            int languageId,
            DateTime changedDateAndTime)
        {
            var id = existingSettings.FindByName(OrdererField.Id);
            MapFieldSetting(updatedSettings.Id, id, languageId, changedDateAndTime);

            var name = existingSettings.FindByName(OrdererField.Name);
            MapFieldSetting(updatedSettings.Name, name, languageId, changedDateAndTime);

            var phone = existingSettings.FindByName(OrdererField.Phone);
            MapFieldSetting(updatedSettings.Phone, phone, languageId, changedDateAndTime);

            var cellPhone = existingSettings.FindByName(OrdererField.CellPhone);
            MapFieldSetting(updatedSettings.CellPhone, cellPhone, languageId, changedDateAndTime);

            var email = existingSettings.FindByName(OrdererField.Email);
            MapFieldSetting(updatedSettings.Email, email, languageId, changedDateAndTime);

            var department = existingSettings.FindByName(OrdererField.Department);
            MapFieldSetting(updatedSettings.Department, department, languageId, changedDateAndTime);
        }

        private static void MapGeneralSettings(
            GeneralFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            int languageId,
            DateTime changedDateAndTime)
        {
            var priority = existingSettings.FindByName(GeneralField.Priority);
            MapFieldSetting(updatedSettings.Priority, priority, languageId, changedDateAndTime);

            var title = existingSettings.FindByName(GeneralField.Title);
            MapFieldSetting(updatedSettings.Title, title, languageId, changedDateAndTime);

            var state = existingSettings.FindByName(GeneralField.Status);
            MapFieldSetting(updatedSettings.Status, state, languageId, changedDateAndTime);

            var system = existingSettings.FindByName(GeneralField.System);
            MapFieldSetting(updatedSettings.System, system, languageId, changedDateAndTime);

            var @object = existingSettings.FindByName(GeneralField.Object);
            MapFieldSetting(updatedSettings.Object, @object, languageId, changedDateAndTime);

            var inventory = existingSettings.FindByName(GeneralField.Inventory);
            MapFieldSetting(updatedSettings.Inventory, inventory, languageId, changedDateAndTime);

            var workingGroup = existingSettings.FindByName(GeneralField.WorkingGroup);
            MapFieldSetting(updatedSettings.WorkingGroup, workingGroup, languageId, changedDateAndTime);

            var administrator = existingSettings.FindByName(GeneralField.Administrator);
            MapFieldSetting(updatedSettings.Administrator, administrator, languageId, changedDateAndTime);

            var finishingDate = existingSettings.FindByName(GeneralField.FinishingDate);
            MapFieldSetting(updatedSettings.FinishingDate, finishingDate, languageId, changedDateAndTime);

            var rss = existingSettings.FindByName(GeneralField.Rss);
            MapFieldSetting(updatedSettings.Rss, rss, languageId, changedDateAndTime);
        }

        private static void MapRegistrationSettings(
            RegistrationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            int languageId,
            DateTime changedDateAndTime)
        {
            var name = existingSettings.FindByName(RegistrationField.Name);
            MapFieldSetting(updatedSettings.Name, name, languageId, changedDateAndTime);

            var phone = existingSettings.FindByName(RegistrationField.Phone);
            MapFieldSetting(updatedSettings.Phone, phone, languageId, changedDateAndTime);

            var email = existingSettings.FindByName(RegistrationField.Email);
            MapFieldSetting(updatedSettings.Email, email, languageId, changedDateAndTime);

            var company = existingSettings.FindByName(RegistrationField.Company);
            MapFieldSetting(updatedSettings.Company, company, languageId, changedDateAndTime);

            var owner = existingSettings.FindByName(RegistrationField.Owner);
            MapFieldSetting(updatedSettings.Owner, owner, languageId, changedDateAndTime);

            var affectedProcesses = existingSettings.FindByName(RegistrationField.AffectedProcesses);
            MapFieldSetting(updatedSettings.AffectedProcesses, affectedProcesses, languageId, changedDateAndTime);

            var affectedDepartments = existingSettings.FindByName(RegistrationField.AffectedDepartments);
            MapFieldSetting(updatedSettings.AffectedDepartments, affectedDepartments, languageId, changedDateAndTime);

            var description = existingSettings.FindByName(RegistrationField.Description);
            MapTextFieldSetting(description, updatedSettings.Description, languageId, changedDateAndTime);

            var businessBenefits = existingSettings.FindByName(RegistrationField.BusinessBenefits);
            MapTextFieldSetting(businessBenefits, updatedSettings.BusinessBenefits, languageId, changedDateAndTime);

            var consequence = existingSettings.FindByName(RegistrationField.Consequence);
            MapTextFieldSetting(consequence, updatedSettings.Consequence, languageId, changedDateAndTime);

            var impact = existingSettings.FindByName(RegistrationField.Impact);
            MapFieldSetting(updatedSettings.Impact, impact, languageId, changedDateAndTime);

            var desiredDate = existingSettings.FindByName(RegistrationField.DesiredDate);
            MapFieldSetting(updatedSettings.DesiredDate, desiredDate, languageId, changedDateAndTime);

            var verified = existingSettings.FindByName(RegistrationField.Verified);
            MapFieldSetting(updatedSettings.Verified, verified, languageId, changedDateAndTime);

            var attachedFiles = existingSettings.FindByName(RegistrationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles, languageId, changedDateAndTime);

            var approval = existingSettings.FindByName(RegistrationField.Approval);
            MapFieldSetting(updatedSettings.Approval, approval, languageId, changedDateAndTime);

            var rejectExplanation = existingSettings.FindByName(RegistrationField.RejectExplanation);
            MapFieldSetting(updatedSettings.RejectExplanation, rejectExplanation, languageId, changedDateAndTime);
        }

        private static void MapAnalyzeSettings(
            AnalyzeFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            int languageId,
            DateTime changedDateAndTime)
        {
            var category = existingSettings.FindByName(AnalyzeField.Category);
            MapFieldSetting(updatedSettings.Category, category, languageId, changedDateAndTime);

            var priority = existingSettings.FindByName(AnalyzeField.Priority);
            MapFieldSetting(updatedSettings.Priority, priority, languageId, changedDateAndTime);

            var responsible = existingSettings.FindByName(AnalyzeField.Responsible);
            MapFieldSetting(updatedSettings.Responsible, responsible, languageId, changedDateAndTime);

            var solution = existingSettings.FindByName(AnalyzeField.Solution);
            MapTextFieldSetting(solution, updatedSettings.Solution, languageId, changedDateAndTime);

            var cost = existingSettings.FindByName(AnalyzeField.Cost);
            MapFieldSetting(updatedSettings.Cost, cost, languageId, changedDateAndTime);

            var yearlyCost = existingSettings.FindByName(AnalyzeField.YearlyCost);
            MapFieldSetting(updatedSettings.YearlyCost, yearlyCost, languageId, changedDateAndTime);

            var estimatedTimeInHours = existingSettings.FindByName(AnalyzeField.EstimatedTimeInHours);
            MapFieldSetting(updatedSettings.EstimatedTimeInHours, estimatedTimeInHours, languageId, changedDateAndTime);

            var risk = existingSettings.FindByName(AnalyzeField.Risk);
            MapTextFieldSetting(risk, updatedSettings.Risk, languageId, changedDateAndTime);

            var startDate = existingSettings.FindByName(AnalyzeField.StartDate);
            MapFieldSetting(updatedSettings.StartDate, startDate, languageId, changedDateAndTime);

            var finishDate = existingSettings.FindByName(AnalyzeField.FinishDate);
            MapFieldSetting(updatedSettings.FinishDate, finishDate, languageId, changedDateAndTime);

            var hasImplementationPlan = existingSettings.FindByName(AnalyzeField.HasImplementationPlan);
            MapFieldSetting(updatedSettings.HasImplementationPlan, hasImplementationPlan, languageId, changedDateAndTime);

            var hasRecoveryPlan = existingSettings.FindByName(AnalyzeField.HasRecoveryPlan);
            MapFieldSetting(updatedSettings.HasRecoveryPlan, hasRecoveryPlan, languageId, changedDateAndTime);

            var attachedFiles = existingSettings.FindByName(AnalyzeField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles, languageId, changedDateAndTime);

            var logs = existingSettings.FindByName(AnalyzeField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs, languageId, changedDateAndTime);

            var approval = existingSettings.FindByName(AnalyzeField.Approval);
            MapFieldSetting(updatedSettings.Approval, approval, languageId, changedDateAndTime);

            var rejectExplanation = existingSettings.FindByName(AnalyzeField.RejectExplanation);
            MapTextFieldSetting(rejectExplanation, updatedSettings.RejectExplanation, languageId, changedDateAndTime);
        }

        private static void MapImplementationSettings(
            ImplementationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            int languageId,
            DateTime changedDateAndTime)
        {
            var status = existingSettings.FindByName(ImplementationField.ImplementationStatus);
            MapFieldSetting(updatedSettings.Status, status, languageId, changedDateAndTime);

            var realStartDate = existingSettings.FindByName(ImplementationField.RealStartDate);
            MapFieldSetting(updatedSettings.RealStartDate, realStartDate, languageId, changedDateAndTime);

            var buildImplemented = existingSettings.FindByName(ImplementationField.BuildImplemented);
            MapFieldSetting(updatedSettings.BuildImplemented, buildImplemented, languageId, changedDateAndTime);

            var implementationPlanUsed = existingSettings.FindByName(ImplementationField.ImplementationPlanUsed);
            MapFieldSetting(updatedSettings.ImplementationPlanUsed, implementationPlanUsed, languageId, changedDateAndTime);

            var deviation = existingSettings.FindByName(ImplementationField.Deviation);
            MapTextFieldSetting(deviation, updatedSettings.Deviation, languageId, changedDateAndTime);

            var recoveryPlanUsed = existingSettings.FindByName(ImplementationField.RecoveryPlanUsed);
            MapFieldSetting(updatedSettings.RecoveryPlanUsed, recoveryPlanUsed, languageId, changedDateAndTime);

            var finishingDate = existingSettings.FindByName(ImplementationField.FinishingDate);
            MapFieldSetting(updatedSettings.FinishingDate, finishingDate, languageId, changedDateAndTime);

            var attachedFiles = existingSettings.FindByName(ImplementationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles, languageId, changedDateAndTime);

            var logs = existingSettings.FindByName(ImplementationField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs, languageId, changedDateAndTime);

            var implementationReady = existingSettings.FindByName(ImplementationField.ImplementationReady);
            MapFieldSetting(updatedSettings.ImplementationReady, implementationReady, languageId, changedDateAndTime);
        }

        private static void MapEvaluationSettings(
            EvaluationFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            int languageId,
            DateTime changedDateAndTime)
        {
            var changeEvaluation = existingSettings.FindByName(EvaluationField.ChangeEvaluation);
            MapTextFieldSetting(changeEvaluation, updatedSettings.Evaluation, languageId, changedDateAndTime);

            var attachedFiles = existingSettings.FindByName(EvaluationField.AttachedFiles);
            MapFieldSetting(updatedSettings.AttachedFiles, attachedFiles, languageId, changedDateAndTime);

            var logs = existingSettings.FindByName(EvaluationField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs, languageId, changedDateAndTime);

            var evaluationReady = existingSettings.FindByName(EvaluationField.EvaluationReady);
            MapFieldSetting(updatedSettings.EvaluationReady, evaluationReady, languageId, changedDateAndTime);
        }

        private static void MapLogSettings(
            LogFieldSettings updatedSettings,
            NamedObjectCollection<ChangeFieldSettingsEntity> existingSettings,
            int languageId,
            DateTime changedDateAndTime)
        {
            var logs = existingSettings.FindByName(LogField.Logs);
            MapFieldSetting(updatedSettings.Logs, logs, languageId, changedDateAndTime);
        }

        private static void MapFieldSetting(
            FieldSetting updatedSetting,
            ChangeFieldSettingsEntity fieldSetting,
            int languageId,
            DateTime changedDateAndTime)
        {
            switch (languageId)
            {
                case LanguageId.Swedish:
                    fieldSetting.Label = updatedSetting.Caption;
                    break;                
                default:
                    fieldSetting.Label_ENG = updatedSetting.Caption;
                    break;
                    //throw new ArgumentOutOfRangeException("languageId");
            }

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
            int languageId,
            DateTime changedDateAndTime)
        {
            switch (languageId)
            {
                case LanguageId.Swedish:
                    fieldSetting.Label = updatedSetting.Caption;
                    break;                                   
                default:
                    fieldSetting.Label_ENG = updatedSetting.Caption;                    
                    break;
                    
            }

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