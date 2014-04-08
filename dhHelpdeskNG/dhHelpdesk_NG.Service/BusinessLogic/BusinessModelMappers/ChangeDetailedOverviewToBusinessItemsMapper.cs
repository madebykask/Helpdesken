namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelMappers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Changes.Fields;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;

    public sealed class ChangeDetailedOverviewToBusinessItemsMapper :
        IBusinessModelsMapper<ChangeDetailedOverview, BusinessItem>
    {
        #region Public Methods and Operators

        public BusinessItem Map(ChangeDetailedOverview businessModel)
        {
            var fields = new List<BusinessItemField>();

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
            var category = new BusinessItemField(AnalyzeField.Category, fields.Category);
            headers.Add(category);

            var priority = new BusinessItemField(AnalyzeField.Priority, fields.Priority);
            headers.Add(priority);

            var responsible = new BusinessItemField(
                AnalyzeField.Responsible,
                fields.Responsible != null ? fields.Responsible.FirstName : null);

            headers.Add(responsible);

            var solution = new BusinessItemField(AnalyzeField.Solution, fields.Solution);
            headers.Add(solution);

            var cost = new BusinessItemField(AnalyzeField.Cost, fields.Cost.ToString());
            headers.Add(cost);

            var yearlyCost = new BusinessItemField(AnalyzeField.YearlyCost, fields.YearlyCost.ToString());
            headers.Add(yearlyCost);

            var estimatedTimeInHours = new BusinessItemField(
                AnalyzeField.EstimatedTimeInHours,
                fields.EstimatedTimeInHours.ToString());

            headers.Add(estimatedTimeInHours);

            var risk = new BusinessItemField(AnalyzeField.Risk, fields.Risk);
            headers.Add(risk);

            var startDate = new BusinessItemField(AnalyzeField.StartDate, fields.StartDate.ToString());
            headers.Add(startDate);

            var finishDate = new BusinessItemField(AnalyzeField.FinishDate, fields.FinishDate.ToString());
            headers.Add(finishDate);

            var hasImplementationPlan = new BusinessItemField(
                AnalyzeField.HasImplementationPlan,
                fields.HasImplementationPlan.ToString());

            headers.Add(hasImplementationPlan);

            var hasRecoveryPlan = new BusinessItemField(
                AnalyzeField.HasRecoveryPlan,
                fields.HasHasRecoveryPlan.ToString());

            headers.Add(hasRecoveryPlan);

            var approval = new BusinessItemField(AnalyzeField.Approval, fields.Approval.ToString());
            headers.Add(approval);

            var rejectExplanation = new BusinessItemField(AnalyzeField.RejectExplanation, fields.RejectExplanation);
            headers.Add(rejectExplanation);
        }

        private static void CreateEvaluationFields(EvaluationFields fields, List<BusinessItemField> headers)
        {
            var changeEvaluation = new BusinessItemField(EvaluationField.ChangeEvaluation, fields.ChangeEvaluation);
            headers.Add(changeEvaluation);

            var evaluationReady = new BusinessItemField(
                EvaluationField.EvaluationReady,
                fields.EvaluationReady.ToString());

            headers.Add(evaluationReady);
        }

        private static void CreateGeneralFields(GeneralFields fields, List<BusinessItemField> headers)
        {
            var priority = new BusinessItemField(GeneralField.Priority, fields.Priority.ToString());
            headers.Add(priority);

            var title = new BusinessItemField(GeneralField.Title, fields.Title);
            headers.Add(title);

            var state = new BusinessItemField(GeneralField.Status, fields.State);
            headers.Add(state);

            var system = new BusinessItemField(GeneralField.System, fields.System);
            headers.Add(system);

            var @object = new BusinessItemField(GeneralField.Object, fields.Object);
            headers.Add(@object);

            var workingGroup = new BusinessItemField(GeneralField.WorkingGroup, fields.WorkingGroup);
            headers.Add(workingGroup);

            var administrator = new BusinessItemField(
                GeneralField.Administrator,
                fields.Administrator != null ? fields.Administrator.ToString() : null);

            headers.Add(administrator);

            var finishingDate = new BusinessItemField(GeneralField.FinishingDate, fields.FinishingDate.ToString());
            headers.Add(finishingDate);

            var rss = new BusinessItemField(GeneralField.Rss, fields.Rss.ToString());
            headers.Add(rss);
        }

        private static void CreateImplementationFields(ImplementationFields fields, List<BusinessItemField> headers)
        {
            var implementationStatus = new BusinessItemField(ImplementationField.Status, fields.Status);
            headers.Add(implementationStatus);

            var realStartDate = new BusinessItemField(
                ImplementationField.RealStartDate,
                fields.RealStartDate.ToString());

            headers.Add(realStartDate);

            var buildImplemented = new BusinessItemField(
                ImplementationField.BuildImplemented,
                fields.BuildImplemented.ToString());

            headers.Add(buildImplemented);

            var implementationPlanUsed = new BusinessItemField(
                ImplementationField.ImplementationPlanUsed,
                fields.ImplementationPlanUsed.ToString());

            headers.Add(implementationPlanUsed);

            var deviation = new BusinessItemField(ImplementationField.Deviation, fields.Deviation);
            headers.Add(deviation);

            var recoveryPlanUsed = new BusinessItemField(
                ImplementationField.RecoveryPlanUsed,
                fields.RecoveryPlanUsed.ToString());

            headers.Add(recoveryPlanUsed);

            var finishingDate = new BusinessItemField(
                ImplementationField.FinishingDate,
                fields.FinishingDate.ToString());

            headers.Add(finishingDate);

            var implementationReady = new BusinessItemField(
                ImplementationField.ImplementationReady,
                fields.ImplementationReady.ToString());

            headers.Add(implementationReady);
        }

        private static void CreateOrdererFields(OrdererFields fields, List<BusinessItemField> headers)
        {
            var id = new BusinessItemField(OrdererField.Id, fields.Id);
            headers.Add(id);

            var name = new BusinessItemField(OrdererField.Name, fields.Name);
            headers.Add(name);

            var phone = new BusinessItemField(OrdererField.Phone, fields.Phone);
            headers.Add(phone);

            var cellPhone = new BusinessItemField(OrdererField.CellPhone, fields.CellPhone);
            headers.Add(cellPhone);

            var email = new BusinessItemField(OrdererField.Email, fields.Email);
            headers.Add(email);

            var department = new BusinessItemField(OrdererField.Department, fields.Department);
            headers.Add(department);
        }

        private static void CreateRegistrationFields(RegistrationFields fields, List<BusinessItemField> headers)
        {
            var description = new BusinessItemField(RegistrationField.Description, fields.Description);
            headers.Add(description);

            var businessBenefits = new BusinessItemField(RegistrationField.BusinessBenefits, fields.BusinessBenefits);
            headers.Add(businessBenefits);

            var consequence = new BusinessItemField(RegistrationField.Consequence, fields.Consequence);
            headers.Add(consequence);

            var impact = new BusinessItemField(RegistrationField.Impact, fields.Impact);
            headers.Add(impact);

            var desiredDate = new BusinessItemField(RegistrationField.DesiredDate, fields.DesiredDate.ToString());
            headers.Add(desiredDate);

            var verified = new BusinessItemField(RegistrationField.Verified, fields.Verified.ToString());
            headers.Add(verified);

            var approval = new BusinessItemField(RegistrationField.Approval, fields.Approval.ToString());
            headers.Add(approval);

            var rejectExplanation = new BusinessItemField(RegistrationField.RejectExplanation, fields.RejectExplanation);
            headers.Add(rejectExplanation);
        }

        #endregion
    }
}