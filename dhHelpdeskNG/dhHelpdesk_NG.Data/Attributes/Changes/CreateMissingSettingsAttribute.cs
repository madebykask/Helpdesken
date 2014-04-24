namespace DH.Helpdesk.Dal.Attributes.Changes
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DH.Helpdesk.Dal.Enums.Changes;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    using PostSharp.Aspects;

    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CreateMissingSettingsAttribute : OnMethodBoundaryAspect
    {
        #region Fields

        private readonly string customerIdParameterName;

        #endregion

        #region Constructors and Destructors

        public CreateMissingSettingsAttribute(string customerIdParameter)
        {
            this.customerIdParameterName = customerIdParameter;
        }

        #endregion

        #region Public Methods and Operators

        public override void OnEntry(MethodExecutionArgs args)
        {
            var methodParameters = args.Method.GetParameters();
            var customerIdParameter = methodParameters.Single(p => p.Name == this.customerIdParameterName);
            var customerIdParameterIndex = customerIdParameter.Position;
            var customerId = (int)args.Arguments[customerIdParameterIndex];

            var dbContext = new DatabaseFactory().Get();
            var settings = dbContext.ChangeFieldSettings;
            var settingNames = settings.Select(s => s.ChangeField).ToList();

            CreateMissingOrdererSettings(customerId, settingNames, settings);
            CreateMissingGeneralSettings(customerId, settingNames, settings);
            CreateMissingRegistrationSettings(customerId, settingNames, settings);
            CreateMissingAnalyzeSettings(customerId, settingNames, settings);
            CreateMissingImplementationSettings(customerId, settingNames, settings);
            CreateMissingEvaluationSettings(customerId, settingNames, settings);
            CreateMissingLogSettings(customerId, settingNames, settings);

            dbContext.Commit();
            base.OnEntry(args);
        }

        #endregion

        #region Methods

        private static ChangeFieldSettingsEntity CreateDefaultSetting(string fieldName, int customerId)
        {
            return new ChangeFieldSettingsEntity
                   {
                       ChangeField = fieldName,
                       CreatedDate = DateTime.Now,
                       Customer_Id = customerId,
                       Label = fieldName,
                       Label_ENG = fieldName,
                       Required = 0,
                       Show = 0,
                       ShowExternal = 0,
                       ShowInList = 0,
                   };
        }

        private static void CreateMissingAnalyzeSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ChangeFieldSettingsEntity> settings)
        {
            CreateSettingIfNeeded(AnalyzeField.Category, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.Priority, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.Responsible, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.Solution, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.Cost, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.YearlyCost, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.EstimatedTimeInHours, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.Risk, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.StartDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.FinishDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.HasImplementationPlan, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.HasRecoveryPlan, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.AttachedFiles, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.Logs, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.Approval, customerId, settingNames, settings);
            CreateSettingIfNeeded(AnalyzeField.RejectExplanation, customerId, settingNames, settings);
        }

        private static void CreateMissingEvaluationSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ChangeFieldSettingsEntity> settings)
        {
            CreateSettingIfNeeded(EvaluationField.ChangeEvaluation, customerId, settingNames, settings);
            CreateSettingIfNeeded(EvaluationField.AttachedFiles, customerId, settingNames, settings);
            CreateSettingIfNeeded(EvaluationField.Logs, customerId, settingNames, settings);
            CreateSettingIfNeeded(EvaluationField.EvaluationReady, customerId, settingNames, settings);
        }

        private static void CreateMissingGeneralSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ChangeFieldSettingsEntity> settings)
        {
            CreateSettingIfNeeded(GeneralField.Priority, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.Title, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.Status, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.System, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.Object, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.Inventory, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.WorkingGroup, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.Administrator, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.FinishingDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralField.Rss, customerId, settingNames, settings);
        }

        private static void CreateMissingImplementationSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ChangeFieldSettingsEntity> settings)
        {
            CreateSettingIfNeeded(ImplementationField.ImplementationStatus, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.RealStartDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.BuildImplemented, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.ImplementationPlanUsed, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.Deviation, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.RecoveryPlanUsed, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.FinishingDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.AttachedFiles, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.Logs, customerId, settingNames, settings);
            CreateSettingIfNeeded(ImplementationField.ImplementationReady, customerId, settingNames, settings);
        }

        private static void CreateMissingLogSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ChangeFieldSettingsEntity> settings)
        {
            CreateSettingIfNeeded(LogField.Logs, customerId, settingNames, settings);
        }

        private static void CreateMissingOrdererSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ChangeFieldSettingsEntity> settings)
        {
            CreateSettingIfNeeded(OrdererField.Id, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrdererField.Name, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrdererField.Phone, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrdererField.CellPhone, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrdererField.Email, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrdererField.Department, customerId, settingNames, settings);
        }

        private static void CreateMissingRegistrationSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ChangeFieldSettingsEntity> settings)
        {
            CreateSettingIfNeeded(RegistrationField.Name, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Phone, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Email, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Company, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Owner, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.AffectedProcesses, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.AffectedDepartments, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Description, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.BusinessBenefits, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Consequence, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Impact, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.DesiredDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Verified, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.AttachedFiles, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.Approval, customerId, settingNames, settings);
            CreateSettingIfNeeded(RegistrationField.RejectExplanation, customerId, settingNames, settings);
        }

        private static void CreateSettingIfNeeded(
            string checkSettingName,
            int customerId,
            List<string> settingNames,
            DbSet<ChangeFieldSettingsEntity> settings)
        {
            var settingExists = settingNames.Any(s => s.ToLower() == checkSettingName.ToLower());
            if (settingExists)
            {
                return;
            }

            var setting = CreateDefaultSetting(checkSettingName, customerId);
            settings.Add(setting);
        }

        #endregion
    }
}