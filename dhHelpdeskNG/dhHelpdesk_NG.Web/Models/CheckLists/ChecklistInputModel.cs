using DH.Helpdesk.BusinessData.Models.Checklists.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class CheckListInputModel
    {
       
        public CheckListInputModel()
        { }

        public CheckListInputModel(
            int checkListId, 
            int wgId, 
            string checklistName,
            List<SelectListItem> workingGroups,
            List<CheckListServiceModel> services
            )
        {
            this.CheckListId = checkListId;
            this.WGId = wgId;
            this.CheckListName = checklistName;
            this.WorkingGroups = workingGroups;
            this.Services = services;
        }

               
        public int CheckListId { get; set; }

        public int? WGId { get; set; }

        public string CheckListName { get; set; }
             
        public List<SelectListItem> WorkingGroups {get; set;}

        public List<CheckListServiceModel> Services { get; set; }

        public List<SelectListItem> ServicesList { get; set; }

        public int? SelectedServiceId { get; set; }

        public string ActionInput { get; set; }

        public string ServiceName { get; set; }
    }
}