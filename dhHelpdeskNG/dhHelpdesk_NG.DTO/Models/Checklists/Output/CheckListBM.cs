using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Checklists.Output
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CheckListBM : INewBusinessModel
    {
        public CheckListBM()
        { }

        public CheckListBM(
                int checkListId,
                int customerId,                
                int? wgId,
                string checklistName,
                DateTime changedDate,
                DateTime createdDate
               )
        {
            this.Id = checkListId;
            this.CustomerId = customerId;            
            this.WorkingGroupId = wgId;
            this.ChecklistName = checklistName;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }


        public int CustomerId { get; private set; }

        [IsId]
        public int Id { get; set; }

        public int? WorkingGroupId { get; private set; }

        public string ChecklistName { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }


    }
}
