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
                int sId,
                int isActive,
                string serviceName,
                List<CheckListServiceBM> servicesList,
                string actionInput,
                IList<CheckListActionsInputModel> actionsList            
            )
            {              
                this.CheckList_Id = checkListId;
                this.Service_Id = serviceId;
                this.SId = sId;
                this.IsActive = isActive;
                this.ServiceName = serviceName;
                this.ServicesList = servicesList.Select(x => new SelectListItem
                {
                    Selected = (x.Id ==  serviceId? true : false),
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(); ;
                this.ActionInput = actionInput;
                this.ActionsList = actionsList;
            }



        public int CheckList_Id { get; set; }   
    
        public int Service_Id { get; set; }

        public int? SId { get; set; }

        public int IsActive { get; set; }

        public string ServiceName { get; set; }

        public List<SelectListItem> ServicesList { get; set; }

        public string ActionInput { get; set; }

        public IList<CheckListActionsInputModel> ActionsList { get; set; }

    }
}