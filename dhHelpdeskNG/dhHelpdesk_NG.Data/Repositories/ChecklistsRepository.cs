// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChecklistsRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IChecklistsRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Linq;
using System.Collections.Generic;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.Checklists.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    
    

    #region CHECKLISTS

    /// <summary>
    /// The ChecklistsRepository interface.
    /// </summary>
    public interface IChecklistsRepository : INewRepository
    {

        void SaveCheckList(CheckListBM checklist);

        List<CheckListBM> GetChecklists(int customerId);
        
        CheckListBM GetChecklist(int checkListId);

    }

    /// <summary>
    /// The checklists repository.
    /// </summary>
    public class ChecklistsRepository : Repository, IChecklistsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecklistsRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public ChecklistsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public CheckListBM GetChecklist(int checkListId)
        {
            var checkListEntity =
                this.DbContext.CheckLists.Where(c => c.Id == checkListId).FirstOrDefault();

            return new CheckListBM(checkListEntity.Customer_Id,
                                   checkListEntity.WorkingGroup_Id, checkListEntity.ChecklistName,
                                   checkListEntity.ChangedDate,
                                   checkListEntity.CreatedDate);
        }

        public List<CheckListBM> GetChecklists(int customerId)
        {           
            var checkListEntitys =
               this.DbContext.Checklists.Where(c => c.Customer_Id == customerId).ToList();
            
            return checkListEntitys.Select(c => new CheckListBM(c.Customer_Id,c.WorkingGroup_Id, c.ChecklistName ,c.ChangedDate, c.CreatedDate)).ToList();          
        }

        public void SaveCheckList(CheckListBM checklist)
        {          
            var checklistsEntity = new CheckListsEntity()
            {                
                Customer_Id = checklist.CustomerId,                
                WorkingGroup_Id = checklist.WorkingGroupId,
                ChecklistName = checklist.ChecklistName,
                CreatedDate = checklist.ChangedDate,
                ChangedDate = checklist.ChangedDate
            };

            //if (checklistsEntity.IsNew())
            //{
               this.DbContext.CheckLists.Add(checklistsEntity);               
            //}
            //else
            //{
            //    this.DbContext.Checklists.(checklistsEntity);
            //}            

            this.InitializeAfterCommit(checklist, checklistsEntity);            
        }
    }

    #endregion

    
}
