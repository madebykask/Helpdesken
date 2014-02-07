namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeHistoryRepository : RepositoryBase<ChangeHistoryEntity>, IChangeHistoryRepository
    {
        public ChangeHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByChangeId(int changeId)
        {
            var histories = this.DataContext.ChangeHistories.Where(h => h.Change_Id == changeId).ToList();
            histories.ForEach(h => this.DataContext.ChangeHistories.Remove(h));
        }

        public List<int> FindIdsByChangeId(int changeId)
        {
            return this.DataContext.ChangeHistories.Where(h => h.Change_Id == changeId).Select(h => h.Id).ToList();
        }

        public List<History> FindByChangeId(int changeId)
        {
            var historyItems =
                this.DataContext.ChangeHistories.Where(h => h.Change_Id == changeId)
                    .Select(
                        h =>
                            new
                            {
                                Id = h.Id,
                                CategoryName = h.ChangeCategory.Name,
                                CategoryId = h.ChangeCategory_Id,
                                ObjectName = h.ChangeObject.Name,
                                ObjectId = h.ChangeObject_Id,
                                PriorityName = h.ChangePriority.Name,
                                PriorityId = h.ChangePriority_Id,
                                StatusName = h.ChangeStatus.Name,
                                StatusId = h.ChangeStatus_Id,
                                RegisteredBy =
                                    h.CreatedByUser_Id.HasValue
                                        ? h.CreatedByUser.FirstName + h.CreatedByUser.SurName
                                        : null,
                                DateAndTime = h.CreatedDate,
                                System = h.System.SystemName,
                                SystemId = h.System_Id,
                                Administrator = h.User_Id.HasValue ? h.User.FirstName + h.User.SurName : null,
                                AdministratorId = h.User_Id,
                                WorkingGroup = h.WorkingGroup_Id.HasValue ? h.WorkingGroup.WorkingGroupName : null,
                                WorkingGroupId = h.WorkingGroup_Id,
                            })
                    .ToList();

            return
                historyItems.Select(
                    i =>
                        new History(
                            i.Id,
                            i.DateAndTime,
                            i.RegisteredBy,
                            i.AdministratorId,
                            i.Administrator,
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
    }
}
