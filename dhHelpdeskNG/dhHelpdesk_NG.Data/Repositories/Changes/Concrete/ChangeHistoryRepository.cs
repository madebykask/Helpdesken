namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeHistoryRepository : Repository, IChangeHistoryRepository
    {
        public ChangeHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
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

        public List<History> FindHistoriesByChangeId(int changeId)
        {
            var histories =
                this.FindByChangeIdCore(changeId)
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
                    .ToList();

            return
                histories.Select(
                    i =>
                        new History(
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

        private IQueryable<ChangeHistoryEntity> FindByChangeIdCore(int changeId)
        {
            return this.DbContext.ChangeHistories.Where(h => h.Change_Id == changeId);
        }
    }
}
