using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Checklists.Output
{

    public sealed class CheckListBM
    {
        public CheckListBM()
        { }

        public CheckListBM(
                int customerId,
                int id,
                int? wgId,
                string checklistName,
                DateTime changedDate,
                DateTime createdDate
               )
        {
            this.CustomerId = customerId;
            this.Id = id;
            this.WorkingGroupId = wgId;
            this.ChecklistName = checklistName;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }


        public int CustomerId { get; set; }
        public int Id { get; set; }     
        public int? WorkingGroupId { get; set; }
        public string ChecklistName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
