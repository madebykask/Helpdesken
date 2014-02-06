namespace dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.Tools;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.Edit;

    public sealed class NewChangeAggregateFactory : INewChangeAggregateFactory
    {
        public NewChangeAggregate Create(
            NewChangeModel model,
            List<WebTemporaryFile> registrationFiles,
            List<WebTemporaryFile> analyzeFiles,
            List<WebTemporaryFile> implementationFiles,
            List<WebTemporaryFile> evaluationFiles,
            int customerId,
            DateTime createdDate)
        {
            var header = CreateHeader(model.Input.Header, createdDate);
            var registration = CreateRegistration(model.Input.Registration, registrationFiles, createdDate);
            var analyze = CreateAnalyze(model.Input.Analyze, analyzeFiles, createdDate);
            var implementation = CreateImplementation(model.Input.Implementation, implementationFiles, createdDate);
            var evaluation = CreateEvaluation(model.Input.Evaluation, evaluationFiles, createdDate);

            return new NewChangeAggregate(customerId, header, registration, analyze, implementation, evaluation);
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

        private static NewRegistrationAggregateFields CreateRegistration(
            RegistrationModel registrationModel,
            List<WebTemporaryFile> webFiles,
            DateTime createdDate)
        {
            var attachedFiles =
                webFiles.Select(f => new NewFile(f.Name, f.Content, Subtopic.Registration, createdDate)).ToList();

            return new NewRegistrationAggregateFields(
                new List<Contact>(),
                registrationModel.OwnerId,
                registrationModel.ProcessAffectedIds,
                registrationModel.DepartmentAffectedIds,
                registrationModel.Description.Value,
                registrationModel.BusinessBenefits.Value,
                registrationModel.Consequence.Value,
                registrationModel.Impact.Value,
                registrationModel.DesiredDate.Value,
                registrationModel.Verified.Value,
                attachedFiles,
                registrationModel.ApprovedValue,
                registrationModel.ApprovedDateAndTime,
                registrationModel.ApprovedUser,
                registrationModel.ApprovableExplanation);
        }

        private static NewAnalyzeAggregateFields CreateAnalyze(
            AnalyzeModel analyzeModel,
            List<WebTemporaryFile> webFiles,
            DateTime createdDate)
        {
            var attachedFiles =
                webFiles.Select(f => new NewFile(f.Name, f.Content, Subtopic.Analyze, createdDate)).ToList();

            return new NewAnalyzeAggregateFields(
                analyzeModel.CategoryId,
                analyzeModel.RelatedChangeIds,
                analyzeModel.PriorityId,
                analyzeModel.ResponsibleId,
                analyzeModel.Solution.Value,
                analyzeModel.Cost.Value,
                analyzeModel.YearlyCost.Value,
                analyzeModel.CurrencyId,
                analyzeModel.TimeEstimatesHours.Value,
                analyzeModel.Risk.Value,
                analyzeModel.StartDate.Value,
                analyzeModel.EndDate.Value,
                analyzeModel.HasImplementationPlan.Value,
                analyzeModel.HasRecoveryPlan.Value,
                attachedFiles,
                analyzeModel.ApprovedValue,
                null,
                null,
                analyzeModel.ChangeRecommendation.Value);
        }

        private static NewImplementationAggregateFields CreateImplementation(
            ImplementationModel implementationModel,
            List<WebTemporaryFile> webFiles,
            DateTime createdDate)
        {
            var attachedFiles =
                webFiles.Select(f => new NewFile(f.Name, f.Content, Subtopic.Implementation, createdDate))
                    .ToList();

            return new NewImplementationAggregateFields(
                implementationModel.ImplementationStatusId,
                implementationModel.RealStartDate,
                implementationModel.FinishingDate,
                implementationModel.BuildImplemented,
                implementationModel.ImplementationPlanUsed,
                implementationModel.ChangeDeviation,
                implementationModel.RecoveryPlanUsed,
                attachedFiles,
                implementationModel.ImplementationReady);
        }

        private static NewEvaluationAggregateFields CreateEvaluation(
            EvaluationModel evaluationModel,
            List<WebTemporaryFile> webFiles,
            DateTime createdDate)
        {
            var attachedFiles =
                webFiles.Select(f => new NewFile(f.Name, f.Content, Subtopic.Evaluation, createdDate)).ToList();

            return new NewEvaluationAggregateFields(
                evaluationModel.ChangeEvaluation,
                attachedFiles,
                evaluationModel.EvaluationReady);
        }
    }
}