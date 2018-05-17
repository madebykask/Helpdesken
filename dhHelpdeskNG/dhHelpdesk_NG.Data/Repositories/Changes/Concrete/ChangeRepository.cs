namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Enums.Changes.Fields;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeRepository : Repository, IChangeRepository
    {
        #region Fields

        private readonly IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview>
            changeEntityToChangeDetailedOverviewMapper;

        private readonly IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper;

        private readonly INewBusinessModelToEntityMapper<NewChange, ChangeEntity> newChangeToChangeEntityMapper;

        private readonly IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper;

        private readonly IUserRepository userRepository;

        private readonly IChangeContactRepository changeContactRepository;

        private readonly IChangeChangeGroupRepository changeChangeGroupRepository;

        private readonly IChangeDepartmentRepository changeDepartmentRepository;

        private readonly IChangeLogRepository changeLogRepository;

        #endregion

        #region Constructors and Destructors

        public ChangeRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ChangeEntity, ChangeDetailedOverview> changeEntityToChangeDetailedOverviewMapper,
            IEntityToBusinessModelMapper<ChangeEntity, Change> changeEntityToChangeMapper,
            INewBusinessModelToEntityMapper<NewChange, ChangeEntity> newChangeToChangeEntityMapper,
            IBusinessModelToEntityMapper<UpdatedChange, ChangeEntity> updatedChangeToChangeEntityMapper, 
            IUserRepository userRepository, 
            IChangeContactRepository changeContactRepository, 
            IChangeChangeGroupRepository changeChangeGroupRepository, 
            IChangeDepartmentRepository changeDepartmentRepository, 
            IChangeLogRepository changeLogRepository)
            : base(databaseFactory)
        {
            this.changeEntityToChangeDetailedOverviewMapper = changeEntityToChangeDetailedOverviewMapper;
            this.changeEntityToChangeMapper = changeEntityToChangeMapper;
            this.newChangeToChangeEntityMapper = newChangeToChangeEntityMapper;
            this.updatedChangeToChangeEntityMapper = updatedChangeToChangeEntityMapper;
            this.userRepository = userRepository;
            this.changeContactRepository = changeContactRepository;
            this.changeChangeGroupRepository = changeChangeGroupRepository;
            this.changeDepartmentRepository = changeDepartmentRepository;
            this.changeLogRepository = changeLogRepository;
        }

        #endregion

        #region Public Methods and Operators

        public List<CustomerChange> GetCustomersChanges(int[] customersIds)
        {
            var entities = this.DbContext.Changes.Where(c => c.Customer_Id.HasValue && 
                                            customersIds.Contains(c.Customer_Id.Value))
                                            .Select(c => new
                                            {
                                                CustomerId = c.Customer_Id.Value, 
                                                ChangeId = c.Id,
                                                c.ChangeStatus,
                                                UserId = c.User_Id
                                            })
                                            .ToList();

            return entities
                    .Select(c => new CustomerChange(
                                c.CustomerId, 
                                c.ChangeId,
                                c.ChangeStatus,
                                c.UserId))
                    .ToList();
        }

        public void AddChange(NewChange change)
        {
            var entity = this.newChangeToChangeEntityMapper.Map(change);
            this.DbContext.Changes.Add(entity);
            this.InitializeAfterCommit(change, entity);
        }

        public void DeleteById(int changeId)
        {
            var change = this.DbContext.Changes.Find(changeId);
            this.DbContext.Changes.Remove(change);
        }

        public Change FindById(int changeId)
        {
            var change = this.DbContext.Changes.Find(changeId);
            return this.changeEntityToChangeMapper.Map(change);
        }

        public Change GetById(int changeId)
        {
            var change = this.DbContext.Changes.Single(c => c.Id == changeId);
            return this.changeEntityToChangeMapper.Map(change);
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var changes = this.FindByCustomerIdCore(customerId).Select(c => new { c.Id, c.ChangeTitle }).ToList();

            return
                changes.Select(c => new ItemOverview(c.ChangeTitle, c.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        public List<ItemOverview> FindOverviewsExcludeSpecified(int customerId, int changeId)
        {
            var changes =
                this.DbContext.Changes.Where(c => c.Customer_Id == customerId && c.Id != changeId)
                    .Select(c => new { c.Id, c.ChangeTitle })
                    .OrderBy(c => c.ChangeTitle)
                    .ToList();

            return
                changes.Select(c => new ItemOverview(c.ChangeTitle, c.Id.ToString(CultureInfo.InvariantCulture)))
                    .ToList();
        }

        public IList<ChangeOverview> GetChanges(int customer)
        {
            var query = from c in this.DbContext.Changes
                        where c.Customer_Id == customer
                        orderby c.OrdererName
                        select new ChangeOverview()
                        {
                            Id = c.Id,
                            ChangeTitle = c.ChangeTitle
                        };
            return query.ToList();
        }

        public SearchResult Search(SearchParameters parameters)
        {
            var searchRequest = this.FindByCustomerIdCore(parameters.CustomerId);

            switch (parameters.Status)
            {
                case ChangeStatus.Active:
                    searchRequest = searchRequest.Where(c => c.ChangeStatus == null || c.ChangeStatus.CompletionStatus == 0);
                    break;
                case ChangeStatus.Finished:
                    searchRequest = searchRequest.Where(c => c.ChangeStatus != null && c.ChangeStatus.CompletionStatus != 0);
                    break;
            }

            if (parameters.StatusIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.StatusIds.Any(i => i == c.ChangeStatus_Id));
            }

            if (parameters.ObjectIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.ObjectIds.Any(i => i == c.ChangeObject_Id));
            }

            if (parameters.OwnerIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.OwnerIds.Any(i => i == c.ChangeGroup_Id));
            }

            if (parameters.AffectedProcessIds.Any())
            {
                searchRequest =
                    searchRequest.Where(
                        c =>
                            this.DbContext.ChangeChangeGroups.Where(cg => cg.Change_Id == c.Id)
                                .Any(cg => parameters.AffectedProcessIds.Contains(cg.ChangeGroup_Id)));
            }

            if (parameters.WorkingGroupIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.WorkingGroupIds.Any(i => i == c.WorkingGroup_Id));
            }

            if (parameters.AdministratorIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.AdministratorIds.Any(i => i == c.User_Id));
            }

            if (parameters.ResponsibleIds.Any())
            {
                searchRequest = searchRequest.Where(c => parameters.ResponsibleIds.Any(i => i == c.ResponsibleUser_Id));
            }

            if (!string.IsNullOrEmpty(parameters.Pharse))
            {
                var pharse = parameters.Pharse != null ? parameters.Pharse.Trim() : parameters.Pharse;

                var administrators = this.userRepository.FindUsersByName(pharse).Select(u => u.Id);
                var responsibles = this.userRepository.FindUsersByName(pharse).Select(u => u.Id);
                var contacts = this.changeContactRepository.FindChangeContacts(pharse).Select(c => c.ChangeId);
                var affectedProcesses = this.changeChangeGroupRepository.FindByName(pharse).Select(cg => cg.Change_Id);
                var affectedDepartments = this.changeDepartmentRepository.FingByName(pharse).Select(d => d.Change_Id);
                var logs = this.changeLogRepository.FingByText(pharse).Select(l => l.Change_Id);

                searchRequest =
                    searchRequest.Where(
                        c =>
                        c.OrdererId.ToLower().Contains(pharse)
                        || c.OrdererName.ToLower().Contains(pharse)
                        || c.OrdererPhone.ToLower().Contains(pharse)
                        || c.OrdererCellPhone.ToLower().Contains(pharse)
                        || c.OrdererEMail.ToLower().Contains(pharse)
                        || (c.OrdererDepartment_Id.HasValue && c.OrdererDepartment.DepartmentName.ToLower().Contains(pharse))
                        || c.ChangeTitle.ToLower().Contains(pharse)
                        || (c.ChangeStatus != null && c.ChangeStatus.ChangeStatus.Contains(pharse))
                        || (c.System != null && c.System.SystemName.ToLower().Contains(pharse))
                        || (c.ChangeObject != null && c.ChangeObject.ChangeObject.ToLower().Contains(pharse))
                        || c.InventoryNumber.Contains(pharse)
                        || (c.WorkingGroup != null && c.WorkingGroup.WorkingGroupName.ToLower().Contains(pharse))
                        || (c.User_Id.HasValue && administrators.Contains(c.User_Id.Value))
                        || (c.ResponsibleUser_Id.HasValue && responsibles.Contains(c.ResponsibleUser_Id.Value))
                        || contacts.Contains(c.Id)
                        || affectedProcesses.Contains(c.Id)
                        || affectedDepartments.Contains(c.Id)
                        || c.ChangeDescription.Contains(pharse)
                        || (c.ChangePriority != null && c.ChangePriority.ChangePriority.ToLower().Contains(pharse))
                        || (c.ImplementationStatus != null && c.ImplementationStatus.ImplementationStatus.ToLower().Contains(pharse))
                        || logs.Contains(c.Id));
            }

            var changesFound = searchRequest.Count();

            if (parameters.SortField != null)
            {
                #region Sorting
                switch (parameters.SortField.SortBy)
                {
                    case SortBy.Ascending:
                        if (parameters.SortField.Name == GeneralField.Priority)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.Prioritisation);
                        }
                        else if (parameters.SortField.Name == GeneralField.Title)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeTitle);
                        }
                        else if (parameters.SortField.Name == GeneralField.Status)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeStatus.ChangeStatus);
                        }
                        else if (parameters.SortField.Name == GeneralField.System)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.System.SystemName);
                        }
                        else if (parameters.SortField.Name == GeneralField.Object)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeObject.ChangeObject);
                        }
                        else if (parameters.SortField.Name == GeneralField.Inventory)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.InventoryNumber);
                        }
                        else if (parameters.SortField.Name == GeneralField.WorkingGroup)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.WorkingGroup.WorkingGroupName);
                        }
                        else if (parameters.SortField.Name == GeneralField.FinishingDate)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.PlannedReadyDate);
                        }
                        else if (parameters.SortField.Name == GeneralField.Rss)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.RSS);
                        }
                        else if (parameters.SortField.Name == OtherField.Id)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.Id);
                        }
                        else if (parameters.SortField.Name == OrdererField.Id)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.OrdererId);
                        }
                        else if (parameters.SortField.Name == OrdererField.Name)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.OrdererName);
                        }
                        else if (parameters.SortField.Name == OrdererField.Phone)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.OrdererPhone);
                        }
                        else if (parameters.SortField.Name == OrdererField.CellPhone)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.OrdererCellPhone);
                        }
                        else if (parameters.SortField.Name == OrdererField.Email)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.OrdererEMail);
                        }
                        else if (parameters.SortField.Name == OrdererField.Department)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.OrdererDepartment.DepartmentName);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Owner)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeGroup.ChangeGroup);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Description)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeDescription);
                        }
                        else if (parameters.SortField.Name == RegistrationField.BusinessBenefits)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeBenefits);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Consequence)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeConsequence);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Impact)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeImpact);
                        }
                        else if (parameters.SortField.Name == RegistrationField.DesiredDate)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.DesiredDate);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Verified)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.Verified);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Approval)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.Approval);
                        }
                        else if (parameters.SortField.Name == RegistrationField.RejectExplanation)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeExplanation);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Category)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeCategory.Name);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Priority)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangePriority.ChangePriority);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Responsible)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ResponsibleUser.SurName);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Solution)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeSolution);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Cost)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.TotalCost);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.YearlyCost)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.YearlyCost);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.EstimatedTimeInHours)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.TimeEstimatesHours);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Risk)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeRisk);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.StartDate)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ScheduledStartTime);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.FinishDate)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ScheduledEndTime);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.HasImplementationPlan)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ImplementationPlan);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.HasRecoveryPlan)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.RecoveryPlan);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Approval)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.AnalysisApproval);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.RejectExplanation)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeRecommendation);
                        }
                        else if (parameters.SortField.Name == ImplementationField.Status)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ImplementationStatus.ImplementationStatus);
                        }
                        else if (parameters.SortField.Name == ImplementationField.RealStartDate)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.RealStartDate);
                        }
                        else if (parameters.SortField.Name == ImplementationField.BuildImplemented)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.BuildImplemented);
                        }
                        else if (parameters.SortField.Name == ImplementationField.ImplementationPlanUsed)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ImplementationPlanUsed);
                        }
                        else if (parameters.SortField.Name == ImplementationField.Deviation)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeDeviation);
                        }
                        else if (parameters.SortField.Name == ImplementationField.RecoveryPlanUsed)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.RecoveryPlanUsed);
                        }
                        else if (parameters.SortField.Name == ImplementationField.FinishingDate)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.FinishingDate);
                        }
                        else if (parameters.SortField.Name == ImplementationField.ImplementationReady)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ImplementationReady);
                        }
                        else if (parameters.SortField.Name == EvaluationField.ChangeEvaluation)
                        {
                            searchRequest = searchRequest.OrderBy(c => c.ChangeEvaluation);
                        }

                        break;
                    case SortBy.Descending:
                        if (parameters.SortField.Name == GeneralField.Priority)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.Prioritisation);
                        }
                        else if (parameters.SortField.Name == GeneralField.Title)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeTitle);
                        }
                        else if (parameters.SortField.Name == GeneralField.Status)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeStatus.ChangeStatus);
                        }
                        else if (parameters.SortField.Name == GeneralField.System)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.System.SystemName);
                        }
                        else if (parameters.SortField.Name == GeneralField.Object)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeObject.ChangeObject);
                        }
                        else if (parameters.SortField.Name == GeneralField.Inventory)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.InventoryNumber);
                        }
                        else if (parameters.SortField.Name == GeneralField.WorkingGroup)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.WorkingGroup.WorkingGroupName);
                        }
                        else if (parameters.SortField.Name == GeneralField.FinishingDate)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.PlannedReadyDate);
                        }
                        else if (parameters.SortField.Name == GeneralField.Rss)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.RSS);
                        }
                        else if (parameters.SortField.Name == OtherField.Id)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.Id);
                        }
                        else if (parameters.SortField.Name == OrdererField.Id)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.OrdererId);
                        }
                        else if (parameters.SortField.Name == OrdererField.Name)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.OrdererName);
                        }
                        else if (parameters.SortField.Name == OrdererField.Phone)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.OrdererPhone);
                        }
                        else if (parameters.SortField.Name == OrdererField.CellPhone)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.OrdererCellPhone);
                        }
                        else if (parameters.SortField.Name == OrdererField.Email)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.OrdererEMail);
                        }
                        else if (parameters.SortField.Name == OrdererField.Department)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.OrdererDepartment.DepartmentName);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Owner)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeGroup.ChangeGroup);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Description)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeDescription);
                        }
                        else if (parameters.SortField.Name == RegistrationField.BusinessBenefits)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeBenefits);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Consequence)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeConsequence);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Impact)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeImpact);
                        }
                        else if (parameters.SortField.Name == RegistrationField.DesiredDate)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.DesiredDate);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Verified)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.Verified);
                        }
                        else if (parameters.SortField.Name == RegistrationField.Approval)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.Approval);
                        }
                        else if (parameters.SortField.Name == RegistrationField.RejectExplanation)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeExplanation);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Category)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeCategory.Name);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Priority)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangePriority.ChangePriority);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Responsible)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ResponsibleUser.SurName);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Solution)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeSolution);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Cost)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.TotalCost);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.YearlyCost)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.YearlyCost);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.EstimatedTimeInHours)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.TimeEstimatesHours);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Risk)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeRisk);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.StartDate)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ScheduledStartTime);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.FinishDate)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ScheduledEndTime);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.HasImplementationPlan)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ImplementationPlan);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.HasRecoveryPlan)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.RecoveryPlan);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.Approval)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.AnalysisApproval);
                        }
                        else if (parameters.SortField.Name == AnalyzeField.RejectExplanation)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeRecommendation);
                        }
                        else if (parameters.SortField.Name == ImplementationField.Status)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ImplementationStatus.ImplementationStatus);
                        }
                        else if (parameters.SortField.Name == ImplementationField.RealStartDate)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.RealStartDate);
                        }
                        else if (parameters.SortField.Name == ImplementationField.BuildImplemented)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.BuildImplemented);
                        }
                        else if (parameters.SortField.Name == ImplementationField.ImplementationPlanUsed)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ImplementationPlanUsed);
                        }
                        else if (parameters.SortField.Name == ImplementationField.Deviation)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeDeviation);
                        }
                        else if (parameters.SortField.Name == ImplementationField.RecoveryPlanUsed)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.RecoveryPlanUsed);
                        }
                        else if (parameters.SortField.Name == ImplementationField.FinishingDate)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.FinishingDate);
                        }
                        else if (parameters.SortField.Name == ImplementationField.ImplementationReady)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ImplementationReady);
                        }
                        else if (parameters.SortField.Name == EvaluationField.ChangeEvaluation)
                        {
                            searchRequest = searchRequest.OrderByDescending(c => c.ChangeEvaluation);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                #endregion
            }

            searchRequest = searchRequest.Take(parameters.SelectCount);
            var changes = searchRequest.ToList();
            var overviews = new List<ChangeDetailedOverview>(changes.Count);
            overviews.AddRange(changes.Select(this.changeEntityToChangeDetailedOverviewMapper.Map));

            return new SearchResult(changesFound, overviews);
        }

        public void Update(UpdatedChange change)
        {
            var entity = this.FindByIdCore(change.Id);
            this.updatedChangeToChangeEntityMapper.Map(change, entity);
        }

        public ChangeOverview GetChangeOverview(int id)
        {
            var entity = this.FindByIdCore(id);
            if (entity == null)
            {
                return null;
            }

            return new ChangeOverview
                       {
                           Id = entity.Id,
                           ChangeTitle = entity.ChangeTitle
                       };
        }

        #endregion

        #region Methods

        private ChangeEntity FindByIdCore(int id)
        {
            return this.DbContext.Changes.Find(id);
        }

        private IQueryable<ChangeEntity> FindByCustomerIdCore(int customerId)
        {
            return this.DbContext.Changes.Where(c => c.Customer_Id == customerId);
        }

        #endregion
    }
}