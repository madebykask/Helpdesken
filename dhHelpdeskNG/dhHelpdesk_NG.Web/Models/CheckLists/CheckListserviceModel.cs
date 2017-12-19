using DH.Helpdesk.BusinessData.Models.Checklists.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class CheckListServiceModel
    {
        public CheckListServiceModel()
        { }


        public CheckListServiceModel(
                int checkListId,
                int serviceId,
                int isActive,
                string serviceName,
                List<CheckListActionsInputModel> actionsList
            )
            {              
                this.CheckList_Id = checkListId;
                this.Id = serviceId;
                this.IsActive = isActive;
                this.ServiceName = serviceName;
                this.ActionsList = actionsList;
            }

        public int CheckList_Id { get; set; }   
    
        public int Id { get; set; }

        public int IsActive { get; set; }

        public string ServiceName { get; set; }

        public List<CheckListActionsInputModel> ActionsList { get; set; }
    }
}