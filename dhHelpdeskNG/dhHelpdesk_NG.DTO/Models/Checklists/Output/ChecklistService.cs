using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Checklists.Output
{
    public sealed class ChecklistService
    {

        public ChecklistService(
                int customerId,
                int id,
                int isActive,
                string checklistName,
                DateTime changedDate,
                DateTime createdDate
               )
        {
            this.CustomerId = customerId;
            this.Id = id;
            this.IsActive = isActive;
            this.ChecklistName = checklistName;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }


        public int CustomerId { get; private set; }
        public int Id { get; private set; }
        public int IsActive { get; set; }
        public string ChecklistName { get; private set; }
        public DateTime ChangedDate { get; private set; }
        public DateTime CreatedDate { get; private set; }
       
        

    }
}
