using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class CheckListActionsInputModel
    {
        public CheckListActionsInputModel() { }

        public CheckListActionsInputModel(
                int serviceId,   
                int actionId,
                int isActive,
                string actionName,
                DateTime changedDate,
                DateTime createdDate
               )
        {
            this.Service_Id = serviceId;
            this.Action_Id = actionId;
            this.IsActive = isActive;
            this.ActionName = actionName;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }


        public int Service_Id { get; set; }

        public int Action_Id { get; set; }

        public int IsActive { get; set; }

        public string ActionName { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}