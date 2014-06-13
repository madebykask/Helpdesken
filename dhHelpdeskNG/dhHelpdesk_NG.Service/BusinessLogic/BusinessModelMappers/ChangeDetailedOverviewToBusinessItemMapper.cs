namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelMappers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes.Fields;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.DisplayValues.Changes;

    public sealed class ChangeDetailedOverviewToBusinessItemMapper :
        IBusinessModelsMapper<ChangeDetailedOverview, BusinessItem>
    {
        #region Public Methods and Operators

        public BusinessItem Map(ChangeDetailedOverview businessModel)
        {
            var fields = new List<BusinessItemField>();
            
            fields.Add(new BusinessItemField("Id", new StringDisplayValue(string.Format("#{0}", businessModel.Id))));

            CreateOrdererFields(businessModel.Orderer, fields);
            CreateGeneralFields(businessModel.General, fields);
            CreateRegistrationFields(businessModel.Registration, fields);
            CreateAnalyzeFields(businessModel.Analyze, fields);
            CreateImplementationFields(businessModel.Implementation, fields);
            CreateEvaluationFields(businessModel.Evaluation, fields);

            return new BusinessItem(fields);
        }

        #endregion

        #region Methods

        private static void CreateAnalyzeFields(AnalyzeFields fields, List<BusinessItemField> headers)
        {
            var category = new BusinessItemField(AnalyzeField.Category, new StringDisplayValue(fields.Category));
            headers.Add(category);

            var priority = new BusinessItemField(AnalyzeField.Priority, new StringDisplayValue(fields.Priority));
            headers.Add(priority);

            var responsible = new BusinessItemField(
                AnalyzeField.Responsible,
                new UserNameDisplayValue(fields.Responsible));

            headers.Add(responsible);

            var solution = new BusinessItemField(AnalyzeField.Solution, new StringDisplayValue(fields.Solution));
            headers.Add(solution);

            var cost = new BusinessItemField(AnalyzeField.Cost, new IntegerDisplayValue(fields.Cost));
            headers.Add(cost);

            var yearlyCost = new BusinessItemField(AnalyzeField.YearlyCost, new IntegerDisplayValue(fields.YearlyCost));
            headers.Add(yearlyCost);

            var estimatedTimeInHours = new BusinessItemField(
                AnalyzeField.EstimatedTimeInHours,
                new IntegerDisplayValue(fields.EstimatedTimeInHours));

            headers.Add(estimatedTimeInHours);

            var risk = new BusinessItemField(AnalyzeField.Risk, new StringDisplayValue(fields.Risk));
            headers.Add(risk);

            var startDate = new BusinessItemField(AnalyzeField.StartDate, new DateTimeDisplayValue(fields.StartDate));
            headers.Add(startDate);

            var finishDate = new BusinessItemField(AnalyzeField.FinishDate, new DateTimeDisplayValue(fields.FinishDate));
            headers.Add(finishDate);

            var hasImplementationPlan = new BusinessItemField(
                AnalyzeField.HasImplementationPlan,
                new BooleanDisplayValue(fields.HasImplementationPlan));

            headers.Add(hasImplementationPlan);

            var hasRecoveryPlan = new BusinessItemField(
                AnalyzeField.HasRecoveryPlan,
                new BooleanDisplayValue(fields.HasHasRecoveryPlan));

            headers.Add(hasRecoveryPlan);

            var approval = new BusinessItemField(AnalyzeField.Approval, new StepStatusDisplayValue(fields.Approval));
            headers.Add(approval);

            var rejectExplanation = new BusinessItemField(
                AnalyzeField.RejectExplanation,
                new StringDisplayValue(fields.RejectExplanation));
            headers.Add(rejectExplanation);
        }

        private static void CreateEvaluationFields(EvaluationFields fields, List<BusinessItemField> headers)
        {
            var changeEvaluation = new BusinessItemField(
                EvaluationField.ChangeEvaluation,
                new StringDisplayValue(fields.ChangeEvaluation));
            headers.Add(changeEvaluation);

            var evaluationReady = new BusinessItemField(
                EvaluationField.EvaluationReady,
                new StepStatusDisplayValue(fields.EvaluationReady));

            headers.Add(evaluationReady);
        }

        private static void CreateGeneralFields(GeneralFields fields, List<BusinessItemField> headers)
        {
            var priority = new BusinessItemField(GeneralField.Priority, new IntegerDisplayValue(fields.Priority));
            headers.Add(priority);

            var title = new BusinessItemField(GeneralField.Title, new StringDisplayValue(fields.Title));
            headers.Add(title);

            var state = new BusinessItemField(GeneralField.Status, new StringDisplayValue(fields.State));
            headers.Add(state);

            var system = new BusinessItemField(GeneralField.System, new StringDisplayValue(fields.System));
            headers.Add(system);

            var @object = new BusinessItemField(GeneralField.Object, new StringDisplayValue(fields.Object));
            headers.Add(@object);

            var workingGroup = new BusinessItemField(
                GeneralField.WorkingGroup,
                new StringDisplayValue(fields.WorkingGroup));
            headers.Add(workingGroup);

            var administrator = new BusinessItemField(
                GeneralField.Administrator,
                new UserNameDisplayValue(fields.Administrator));

            headers.Add(administrator);

            var finishingDate = new BusinessItemField(
                GeneralField.FinishingDate,
                new DateTimeDisplayValue(fields.FinishingDate));
            headers.Add(finishingDate);

            var rss = new BusinessItemField(GeneralField.Rss, new BooleanDisplayValue(fields.Rss));
            headers.Add(rss);
        }

        private static void CreateImplementationFields(ImplementationFields fields, List<BusinessItemField> headers)
        {
            var implementationStatus = new BusinessItemField(
                ImplementationField.Status,
                new StringDisplayValue(fields.Status));
            headers.Add(implementationStatus);

            var realStartDate = new BusinessItemField(
                ImplementationField.RealStartDate,
                new DateTimeDisplayValue(fields.RealStartDate));

            headers.Add(realStartDate);

            var buildImplemented = new BusinessItemField(
                ImplementationField.BuildImplemented,
                new BooleanDisplayValue(fields.BuildImplemented));

            headers.Add(buildImplemented);

            var implementationPlanUsed = new BusinessItemField(
                ImplementationField.ImplementationPlanUsed,
                new BooleanDisplayValue(fields.ImplementationPlanUsed));

            headers.Add(implementationPlanUsed);

            var deviation = new BusinessItemField(
                ImplementationField.Deviation,
                new StringDisplayValue(fields.Deviation));
            headers.Add(deviation);

            var recoveryPlanUsed = new BusinessItemField(
                ImplementationField.RecoveryPlanUsed,
                new BooleanDisplayValue(fields.RecoveryPlanUsed));

            headers.Add(recoveryPlanUsed);

            var finishingDate = new BusinessItemField(
                ImplementationField.FinishingDate,
                new DateTimeDisplayValue(fields.FinishingDate));

            headers.Add(finishingDate);

            var implementationReady = new BusinessItemField(
                ImplementationField.ImplementationReady,
                new StepStatusDisplayValue(fields.ImplementationReady));

            headers.Add(implementationReady);
        }

        private static void CreateOrdererFields(OrdererFields fields, List<BusinessItemField> headers)
        {
            var id = new BusinessItemField(OrdererField.Id, new StringDisplayValue(fields.Id));
            headers.Add(id);

            var name = new BusinessItemField(OrdererField.Name, new StringDisplayValue(fields.Name));
            headers.Add(name);

            var phone = new BusinessItemField(OrdererField.Phone, new StringDisplayValue(fields.Phone));
            headers.Add(phone);

            var cellPhone = new BusinessItemField(OrdererField.CellPhone, new StringDisplayValue(fields.CellPhone));
            headers.Add(cellPhone);

            var email = new BusinessItemField(OrdererField.Email, new StringDisplayValue(fields.Email));
            headers.Add(email);

            var department = new BusinessItemField(OrdererField.Department, new StringDisplayValue(fields.Department));
            headers.Add(department);
        }

        private static void CreateRegistrationFields(RegistrationFields fields, List<BusinessItemField> headers)
        {
            var description = new BusinessItemField(
                RegistrationField.Description,
                new StringDisplayValue(fields.Description));
            headers.Add(description);

            var businessBenefits = new BusinessItemField(
                RegistrationField.BusinessBenefits,
                new StringDisplayValue(fields.BusinessBenefits));
            headers.Add(businessBenefits);

            var consequence = new BusinessItemField(
                RegistrationField.Consequence,
                new StringDisplayValue(fields.Consequence));
            headers.Add(consequence);

            var impact = new BusinessItemField(RegistrationField.Impact, new StringDisplayValue(fields.Impact));
            headers.Add(impact);

            var desiredDate = new BusinessItemField(
                RegistrationField.DesiredDate,
                new DateTimeDisplayValue(fields.DesiredDate));
            headers.Add(desiredDate);

            var verified = new BusinessItemField(RegistrationField.Verified, new BooleanDisplayValue(fields.Verified));
            headers.Add(verified);

            var approval = new BusinessItemField(
                RegistrationField.Approval,
                new StepStatusDisplayValue(fields.Approval));
            headers.Add(approval);

            var rejectExplanation = new BusinessItemField(
                RegistrationField.RejectExplanation,
                new StringDisplayValue(fields.RejectExplanation));
            headers.Add(rejectExplanation);
        }

        #endregion
    }
}