using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.Checklists.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    

    #region CHECKLIST

    public interface IChecklistRepository : IRepository<Checklist>
    {
    }

    public class ChecklistRepository : RepositoryBase<Checklist>, IChecklistRepository
    {
        public ChecklistRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHECKLISTACTION

    public interface IChecklistActionRepository : INewRepository
    {
        List<CheckListActionBM> GetCheckListActions(int serviceId);
        void SaveCheckListAction(CheckListActionBM checklistAction);
        void DeleteAction(int actionId);
        void UpdateAction(CheckListActionBM checkListAction);
        int GetCheckListIdByAction(int id);
    }

    public class ChecklistActionRepository : Repository, IChecklistActionRepository
    {
        public ChecklistActionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


        public List<CheckListActionBM> GetCheckListActions(int serviceId)
        {

            var checkListActionsEntitys =
                   this.DbContext.CheckListActions.Where(c => c.ChecklistService_Id == serviceId).ToList();

            if (checkListActionsEntitys != null)
            {

                return checkListActionsEntitys.Select(c => new CheckListActionBM(c.Id, c.ChecklistService_Id, c.IsActive, c.Name, c.ChangedDate, c.CreatedDate)).ToList();
            }
            else
                return null;
        }


        public void SaveCheckListAction(CheckListActionBM checklistAction)
        {
            var New_CheckListAction = new ChecklistAction()
            {
                ChecklistService_Id = checklistAction.Service_Id,
                IsActive = checklistAction.IsActive,
                Name = checklistAction.ActionName,
                CreatedDate = checklistAction.ChangedDate,
                ChangedDate = checklistAction.ChangedDate
            };


            this.DbContext.CheckListActions.Add(New_CheckListAction);
            this.InitializeAfterCommit(checklistAction, New_CheckListAction);
        }

        public void DeleteAction(int actionId)
        {
            if (actionId > 0)
            {
                var checkListActionEntity = DbContext.CheckListActions.Find(actionId);
                if (checkListActionEntity != null)
                    DbContext.CheckListActions.Remove(checkListActionEntity);
            }
        }

        public void UpdateAction(CheckListActionBM checkListAction)
        {
            var checkListActionEntity = DbContext.CheckListActions.Find(checkListAction.Id);
            if (checkListActionEntity != null)
            {
                checkListActionEntity.Name = checkListAction.ActionName;
                checkListActionEntity.ChangedDate = checkListAction.ChangedDate;
                checkListActionEntity.IsActive = checkListAction.IsActive;
            }
        }

        public int GetCheckListIdByAction(int id)
        {
            var action = DbContext.CheckListActions
                .Include(x => x.ChecklistService)
                .FirstOrDefault(x => x.Id == id);
            if (action != null)
            {
                return action.ChecklistService.CheckList_Id;
            }
            return 0;
        }
    }

    #endregion

    #region CHECKLISTROW

    public interface IChecklistRowRepository : IRepository<ChecklistRow>
    {
    }

    public class ChecklistRowRepository : RepositoryBase<ChecklistRow>, IChecklistRowRepository
    {
        public ChecklistRowRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CHECKLISTSERVICE

    public interface ICheckListServiceRepository : INewRepository
    {
        List<CheckListServiceBM> GetCheckListServices(int checkListId);
        void SaveCheckListService(CheckListServiceBM checkListAService);
        void UpdateCheckListService(CheckListServiceBM checkListService);
    }

    public class CheckListServiceRepository : Repository, ICheckListServiceRepository
    {
        public CheckListServiceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<CheckListServiceBM> GetCheckListServices(int checkListId)
        {
            
            var checkListServiceEntitys =
                   this.DbContext.CheckListServices.Where(c => c.CheckList_Id == checkListId).ToList();

            if (checkListServiceEntitys != null)
            {
               
                return checkListServiceEntitys.Select(c => new CheckListServiceBM(c.Customer_Id, c.CheckList_Id, c.Id, c.IsActive, c.Name, c.ChangedDate, c.CreatedDate)).ToList();
            }
            else
                return null;
        }


        public void SaveCheckListService(CheckListServiceBM checkListService)
        {
            var CheckListServiceEntity = new ChecklistService()
            {
                Customer_Id = checkListService.CustomerId,
                CheckList_Id = checkListService.CheckListId,
                IsActive = checkListService.IsActive,
                Name = checkListService.Name,
                CreatedDate = checkListService.ChangedDate,
                ChangedDate = checkListService.ChangedDate
            };


            this.DbContext.CheckListServices.Add(CheckListServiceEntity);

            this.InitializeAfterCommit(checkListService, CheckListServiceEntity);
        }

        public void UpdateCheckListService(CheckListServiceBM checkListService)
        {
            var checkListServiceEntity = DbContext.CheckListServices.FirstOrDefault(x => x.Id == checkListService.Id);
            if (checkListServiceEntity != null)
            {
                checkListServiceEntity.Name = checkListService.Name;
                checkListServiceEntity.ChangedDate = checkListService.ChangedDate;
                checkListServiceEntity.IsActive = checkListService.IsActive;
            }
        }
    }

    #endregion
}
