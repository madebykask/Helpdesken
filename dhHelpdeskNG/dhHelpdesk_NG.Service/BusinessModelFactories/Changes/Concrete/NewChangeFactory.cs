namespace dhHelpdesk_NG.Service.BusinessModelFactories.Changes.Concrete
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChangeAggregate;

    using NewAnalyzeFields = dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange.NewAnalyzeFields;
    using NewChangeHeader = dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange.NewChangeHeader;
    using NewEvaluationFields = dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange.NewEvaluationFields;
    using NewImplementationFields = dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange.NewImplementationFields;
    using NewRegistrationFields = dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange.NewRegistrationFields;

    public sealed class NewChangeFactory : INewChangeFactory
    {
        public NewChange Create(NewChangeAggregate newChange)
        {
            var header = CreateHeader(newChange.Header);
            var registration = CreateRegistration(newChange.Registration);
            var analyze = CreateAnalyze(newChange.Analyze);
            var implementation = CreateImplementation(newChange.Implementation);
            var evalution = CreateEvaluation(newChange.Evaluation);

            return new NewChange(newChange.CustomerId, header, registration, analyze, implementation, evalution);
        }

        private static NewChangeHeader CreateHeader(NewChangeAggregateHeader header)
        {
            return new NewChangeHeader(
                header.Id,
                header.Name,
                header.Phone,
                header.CellPhone,
                header.Email,
                header.DepartmentId,
                header.Title,
                header.StatusId,
                header.SystemId,
                header.ObjectId,
                header.WorkingGroupId,
                header.AdministratorId,
                header.FinishingDate,
                header.CreatedDate,
                header.Rss);
        }

        private static NewRegistrationFields CreateRegistration(NewRegistrationAggregateFields registrationFields)
        {
            return new NewRegistrationFields(
                registrationFields.OwnerId,
                registrationFields.Description,
                registrationFields.BusinessBenefits,
                registrationFields.Consequece,
                registrationFields.Impact,
                registrationFields.DesiredDate,
                registrationFields.Verified,
                registrationFields.Approved,
                registrationFields.ApprovedUser,
                registrationFields.ApprovedDateAndTime,
                registrationFields.ApprovalExplanation);
        }

        private static NewAnalyzeFields CreateAnalyze(NewAnalyzeAggregateFields analyzeFields)
        {
            return new NewAnalyzeFields(
                analyzeFields.CategoryId,
                analyzeFields.PriorityId,
                analyzeFields.ResponsibleId,
                analyzeFields.Solution,
                analyzeFields.Cost,
                analyzeFields.YearlyCost,
                analyzeFields.CurrencyId,
                analyzeFields.TimeEstimatesHours,
                analyzeFields.Risk,
                analyzeFields.StartDate,
                analyzeFields.EndDate,
                analyzeFields.HasImplementationPlan,
                analyzeFields.HasRecoveryPlan,
                analyzeFields.Approved,
                analyzeFields.ApprovedDateAndTime,
                analyzeFields.ApprovedUser,
                analyzeFields.ChangeRecommendation);
        }

        private static NewImplementationFields CreateImplementation(NewImplementationAggregateFields implementationFields)
        {
            return new NewImplementationFields(
                implementationFields.ImplementationStatusId,
                implementationFields.RealStartDate,
                implementationFields.FinishingDate,
                implementationFields.BuildImplemented,
                implementationFields.ImplementationPlanUsed,
                implementationFields.ChangeDeviation,
                implementationFields.RecoveryPlanUsed,
                implementationFields.ImplementationReady);
        }

        private static NewEvaluationFields CreateEvaluation(NewEvaluationAggregateFields evaluationFields)
        {
            return new NewEvaluationFields(evaluationFields.ChangeEvaluation, evaluationFields.EvaluationReady);
        }
    }
}
