using DH.Helpdesk.BusinessData.Models.Checklists.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class CheckListserviceModel
    {
        public CheckListserviceModel()
        { }


        public CheckListserviceModel
                       (int checkListId, int customerId, int serciceId, int isActive, string serviceName, DateTime changedDate, DateTime createdDate,
                            List<ChecklistServiceBM> serviceslist,
                            ServicesActionsModel listOfActions)
        {
            this.Customer_Id = customerId;
            this.CheckList_Id = checkListId;
            this.Service_Id = serciceId;
            this.IsActive = isActive;
            this.ServiceName = serviceName;
            this.ServicesList = serviceslist.Select( s => new SelectListItem 
            { Text = s.Name,              
              Value = s.Id.ToString()
            }).ToList();            
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
            this.ListOfActions = listOfActions;
                            }



        public int CheckList_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Service_Id { get; set; }
        public int IsActive { get; set; }
        public string ServiceName { get; set; }
        public List<SelectListItem> ServicesList { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public ServicesActionsModel ListOfActions { get; set; }

    }
}