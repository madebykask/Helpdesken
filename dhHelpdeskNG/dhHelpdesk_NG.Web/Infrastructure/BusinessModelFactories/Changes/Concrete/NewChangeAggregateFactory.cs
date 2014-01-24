namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.InputModel;

    public sealed class NewChangeAggregateFactory : INewChangeAggregateFactory
    {
        public NewChangeAggregate Create(NewChangeModel model, DateTime createdDate)
        {
            var header = CreateHeader(model.Input.Header, createdDate);
            var registration = CreateRegistration(model.Input.Registration);
            var analyze = CreateAnalyze(model.Input.Analyze);
            var implementation = CreateImplementation(model.Input.Implementation);
            var evaluation = CreateEvaluation(model.Input.Evaluation);

            return new NewChangeAggregate(header, registration, analyze, implementation, evaluation);
        }

        private static NewChangeAggregateHeader CreateHeader(ChangeHeaderModel headerModel, DateTime createdDate)
        {
            return new NewChangeAggregateHeader(
                headerModel.Id,
                headerModel.Name,
                headerModel.Phone,
                headerModel.CellPhone,
                headerModel.Email,
                headerModel.DepartmentId,
                headerModel.Title,
                headerModel.StatusId,
                headerModel.SystemId,
                headerModel.ObjectId,
                headerModel.WorkingGroupId,
                headerModel.AdministratorId,
                headerModel.FinishingDate,
                createdDate,
                headerModel.Rss);
        }

        private static NewRegistrationAggregateFields CreateRegistration(RegistrationModel registrationModel)
        {
            return new NewRegistrationAggregateFields(
                new List<Contact>(),
                registrationModel.OwnerId,
                registrationModel.ProcessAffectedIds,
                registrationModel.DepartmentAffectedIds,
                registrationModel.Description,
                registrationModel.BusinessBenefits,
                registrationModel.Consequence,
                registrationModel.Impact,
                registrationModel.DesiredDate,
                registrationModel.Verified,
                registrationModel.ApprovedValue,
                registrationModel.ApprovedDateAndTime,
                registrationModel.ApprovedUser,
                registrationModel.ApprovableExplanation);
        }

        private static NewAnalyzeAggregateFields CreateAnalyze(AnalyzeModel analyzeModel)
        {
            return new NewAnalyzeAggregateFields(
                analyzeModel.CategoryId,
                analyzeModel.PriorityId,
                analyzeModel.ResponsibleId,
                analyzeModel.Solution,
                analyzeModel.Cost,
                analyzeModel.YearlyCost,
                analyzeModel.CurrencyId,
                analyzeModel.TimeEstimatesHours,
                analyzeModel.Risk,
                analyzeModel.StartDate,
                analyzeModel.EndDate,
                analyzeModel.HasImplementationPlan,
                analyzeModel.HasRecoveryPlan,
                analyzeModel.ApprovedValue,
                null,
                null,
                analyzeModel.ChangeRecommendation);
        }

        private static NewImplementationAggregateFields CreateImplementation(ImplementationModel implementationModel)
        {
            return new NewImplementationAggregateFields(
                implementationModel.ImplementationStatusId,
                implementationModel.RealStartDate,
                implementationModel.FinishingDate,
                implementationModel.BuildImplemented,
                implementationModel.ImplementationPlanUsed,
                implementationModel.ChangeDeviation,
                implementationModel.RecoveryPlanUsed,
                implementationModel.ImplementationReady);
        }

        private static NewEvaluationAggregateFields CreateEvaluation(EvaluationModel evaluationModel)
        {
            return new NewEvaluationAggregateFields(evaluationModel.ChangeEvaluation, evaluationModel.EvaluationReady);
        }
    }
}