namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeAggregate;
    using dhHelpdesk_NG.DTO.Enums.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;
    using dhHelpdesk_NG.Web.Models.Changes.InputModel;

    public sealed class ChangeModelFactory : IChangeModelFactory
    {
        public ChangeModel Create(ChangeAggregate change, ChangeOptionalData optionalData)
        {
            var header = CreateHeader(change, optionalData);
            var registration = CreateRegistration(change, optionalData);
            var analyze = CreateAnalyze(change, optionalData);
            var implementation = CreateImplementation(change, optionalData);
            var evaluation = CreateEvaluation(change);
            var history = CreateHistory(change);

            var inputModel = new InputModel(header, registration, analyze, implementation, evaluation, history);
            return new ChangeModel(change.Id, inputModel);
        }

        private static ChangeHeaderModel CreateHeader(ChangeAggregate change, ChangeOptionalData optionalData)
        {
            var departmentList = new SelectList(optionalData.Departments, "Value", "Name", change.Header.DepartmentId);
            var statusList = new SelectList(optionalData.Statuses, "Value", "Name", change.Header.StatusId);
            var systemList = new SelectList(optionalData.Systems, "Value", "Name", change.Header.SystemId);
            var objectList = new SelectList(optionalData.Objects, "Value", "Name", change.Header.ObjectId);
            var workingGroupList = new SelectList(optionalData.WorkingGroups, "Value", "Name", change.Header.WorkingGroupId);
            var administratorList = new SelectList(optionalData.Administrators, "Value", "Name", change.Header.AdministratorId);

            return new ChangeHeaderModel(
                change.Header.Id,
                change.Header.Name,
                change.Header.Phone,
                change.Header.CellPhone,
                change.Header.Email,
                departmentList,
                change.Header.Title,
                statusList,
                systemList,
                objectList,
                workingGroupList,
                administratorList,
                change.Header.FinishingDate,
                change.Header.CreatedDate,
                change.Header.ChangedDate,
                change.Header.Rss);
        }

        private static RegistrationModel CreateRegistration(
            ChangeAggregate change, 
            ChangeOptionalData optionalData)
        {
            var ownerList = new SelectList(optionalData.Owners, "Value", "Name", change.Registration.OwnerId);

            var processAffectedList = new MultiSelectList(
                optionalData.ProcessesAffected,
                "Value",
                "Name",
                change.Registration.ProcessesAffectedIds);

            var attachedFilesContainer =
                new AttachedFilesContainerModel(change.Id.ToString(CultureInfo.InvariantCulture), Subtopic.Registration);

            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approved", Enums.TranslationSource.TextTranslation);
            approveItem.Value = AnalyzeApproveResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = AnalyzeApproveResult.Rejected.ToString();

            var approvedItems = new List<object> { approveItem, rejectItem };
            var approvedList = new SelectList(approvedItems, "Value", "Text");

            return new RegistrationModel(
                change.Id.ToString(CultureInfo.InvariantCulture),
                ownerList,
                processAffectedList,
                processAffectedList,
                change.Registration.Description,
                change.Registration.BusinessBenefits,
                change.Registration.Consequece,
                change.Registration.Impact,
                change.Registration.DesiredDate,
                change.Registration.Verified,
                attachedFilesContainer,
                approvedList,
                change.Registration.ApprovalExplanation,
                change.Registration.ApprovedDateAndTime,
                change.Registration.ApprovedUser);
        }

        private static AnalyzeModel CreateAnalyze(
            ChangeAggregate change,
            ChangeOptionalData optionalData)
        {
            var categoryList = new SelectList(optionalData.Categories, "Value", "Name");
            var relatedChangeList = new MultiSelectList(optionalData.RelatedChanges, "Value", "Name");
            var priorityList = new SelectList(optionalData.Priorities, "Value", "Name");
            var responsibleList = new SelectList(optionalData.Responsibles, "Value", "Name");
            var currencyList = new SelectList(optionalData.Currencies, "Value", "Name");

            var attachedFilesContainer =
                new AttachedFilesContainerModel(change.Id.ToString(CultureInfo.InvariantCulture), Subtopic.Analyze);

            var approveItem = new SelectListItem();
            approveItem.Text = Translation.Get("Approved", Enums.TranslationSource.TextTranslation);
            approveItem.Value = AnalyzeApproveResult.Approved.ToString();

            var rejectItem = new SelectListItem();
            rejectItem.Text = Translation.Get("Reject", Enums.TranslationSource.TextTranslation);
            rejectItem.Value = AnalyzeApproveResult.Rejected.ToString();

            var approvedItems = new List<object> { approveItem, rejectItem };
            var approvedList = new SelectList(approvedItems, "Value", "Text");

            return new AnalyzeModel(
                categoryList,
                relatedChangeList,
                priorityList,
                responsibleList,
                change.Analyze.Solution,
                change.Analyze.Cost,
                change.Analyze.YearlyCost,
                currencyList,
                change.Analyze.TimeEstimatesHours,
                change.Analyze.Risk,
                change.Analyze.StartDate,
                change.Analyze.EndDate,
                change.Analyze.HasImplementationPlan,
                change.Analyze.HasRecoveryPlan,
                attachedFilesContainer,
                approvedList,
                change.Analyze.ChangeRecommendation);
        }

        private static ImplementationModel CreateImplementation(
            ChangeAggregate change,
            ChangeOptionalData optionalData)
        {
            var implementationStatusList = new SelectList(optionalData.ImplementationStatuses, "Value", "Name");

            var attachedFilesContainer =
                new AttachedFilesContainerModel(
                    change.Id.ToString(CultureInfo.InvariantCulture),
                    Subtopic.Implementation);

            return new ImplementationModel(
                implementationStatusList,
                change.Implementation.RealStartDate,
                change.Implementation.FinishingDate,
                change.Implementation.BuildImplemented,
                change.Implementation.ImplementationPlanUsed,
                change.Implementation.ChangeDeviation,
                change.Implementation.RecoveryPlanUsed,
                attachedFilesContainer,
                change.Implementation.ImplementationReady);
        }

        private static EvaluationModel CreateEvaluation(ChangeAggregate change)
        {
            var attachedFilesContainer =
                new AttachedFilesContainerModel(change.Id.ToString(CultureInfo.InvariantCulture), Subtopic.Evaluation);

            return new EvaluationModel(
                change.Evaluation.ChangeEvaluation,
                attachedFilesContainer,
                change.Evaluation.EvaluationReady);
        }

        private static HistoryModel CreateHistory(ChangeAggregate change)
        {
            var historyItems = new List<HistoryItemModel>(change.Histories.Count);

            foreach (var history in change.Histories)
            {
                var diff =
                    history.History.Select(h => new FieldDifferenceModel(h.FieldName, h.OldValue, h.NewValue)).ToList();

                var historyItem = new HistoryItemModel(
                    history.DateAndTime,
                    history.RegisteredBy,
                    history.Log,
                    diff,
                    history.Emails);

                historyItems.Add(historyItem);
            }

            return new HistoryModel(historyItems);
        }
    }
}