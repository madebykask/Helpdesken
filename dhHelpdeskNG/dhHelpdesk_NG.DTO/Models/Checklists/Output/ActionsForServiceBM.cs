using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Checklists.Output
{
    public sealed class ActionsForServiceBM
    {

        public ActionsForServiceBM(
                int checklistServiceId,   
                int actionId,
                int isActive,
                string actionName,
                DateTime changedDate,
                DateTime createdDate
               )
        {
            this.ChecklistService_Id = checklistServiceId;
            this.Action_Id = actionId;
            this.IsActive = isActive;
            this.ActionName = actionName;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }


        public int ChecklistService_Id { get; private set; }
        public int Action_Id { get; private set; }
        public int IsActive { get; private set; }
        public string ActionName { get; private set; }
        public DateTime ChangedDate { get; private set; }
        public DateTime CreatedDate { get; private set; }

    }
}
