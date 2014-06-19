namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeHistoryRepository : Repository, IChangeHistoryRepository
    {
        private readonly INewBusinessModelToEntityMapper<History, ChangeHistoryEntity> historyToChangeHistoryEntityMapper;

        public ChangeHistoryRepository(
            IDatabaseFactory databaseFactory,
            INewBusinessModelToEntityMapper<History, ChangeHistoryEntity> historyToChangeHistoryEntityMapper)
            : base(databaseFactory)
        {
            this.historyToChangeHistoryEntityMapper = historyToChangeHistoryEntityMapper;
        }

        public void DeleteByChangeId(int changeId)
        {
            var histories = this.FindByChangeIdCore(changeId).ToList();
            histories.ForEach(h => this.DbContext.ChangeHistories.Remove(h));
        }

        public List<int> FindIdsByChangeId(int changeId)
        {
            return this.FindByChangeIdCore(changeId).Select(h => h.Id).ToList();
        }

        public List<HistoryOverview> FindHistoriesByChangeId(int changeId)
        {
            var histories =
                this.FindByChangeIdCore(changeId)
                    .Where(h => h.CreatedByUser_Id.HasValue)
                    .Select(
                        h =>
                            new
                            {
                                Id = h.Id,
                                CategoryName = h.ChangeCategory.Name,
                                CategoryId = h.ChangeCategory_Id,
                                ObjectName = h.ChangeObject.ChangeObject,
                                ObjectId = h.ChangeObject_Id,
                                PriorityName = h.ChangePriority.ChangePriority,
                                PriorityId = h.ChangePriority_Id,
                                StatusName = h.ChangeStatus.ChangeStatus,
                                StatusId = h.ChangeStatus_Id,
                                RegisteredByUserId = h.CreatedByUser_Id,
                                RegisteredByUserFirstName =
                                    h.CreatedByUser_Id.HasValue ? h.CreatedByUser.FirstName : null,
                                RegisteredByUserLastName =
                                    h.CreatedByUser_Id.HasValue ? h.CreatedByUser.SurName : null,
                                DateAndTime = h.CreatedDate,
                                System = h.System.SystemName,
                                SystemId = h.System_Id,
                                AdministratorId = h.User_Id,
                                AdministratorFirstName = h.User_Id.HasValue ? h.User.FirstName : null,
                                AdministratorLastName = h.User_Id.HasValue ? h.User.SurName : null,
                                WorkingGroup = h.WorkingGroup_Id.HasValue ? h.WorkingGroup.WorkingGroupName : null,
                                WorkingGroupId = h.WorkingGroup_Id,
                            })
                    .OrderByDescending(h => h.DateAndTime)
                    .ToList();

            return
                histories.Select(
                    i =>
                        new HistoryOverview(
                            i.Id,
                            i.DateAndTime,
                            i.RegisteredByUserId.HasValue
                                ? new UserName(i.RegisteredByUserFirstName, i.RegisteredByUserLastName)
                                : null,
                            i.AdministratorId,
                            i.AdministratorId.HasValue
                                ? new UserName(i.AdministratorFirstName, i.AdministratorLastName)
                                : null,
                            i.ObjectId,
                            i.ObjectName,
                            i.PriorityId,
                            i.PriorityName,
                            i.WorkingGroupId,
                            i.WorkingGroup,
                            i.SystemId,
                            i.System,
                            i.CategoryId,
                            i.CategoryName,
                            i.StatusId,
                            i.StatusName)).ToList();
        }

        public void AddHistory(History history)
        {
            var entity = this.historyToChangeHistoryEntityMapper.Map(history);
            this.DbContext.ChangeHistories.Add(entity);
            this.InitializeAfterCommit(history, entity);
        }

        private IQueryable<ChangeHistoryEntity> FindByChangeIdCore(int changeId)
        {
            return this.DbContext.ChangeHistories.Where(h => h.Change_Id == changeId);
        }
    }
}