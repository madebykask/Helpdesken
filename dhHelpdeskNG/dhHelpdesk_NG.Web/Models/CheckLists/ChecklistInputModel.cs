using DH.Helpdesk.BusinessData.Models.Checklists.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class ChecklistInputModel
    {
       
        public ChecklistInputModel()
        { }

        public ChecklistInputModel(
            int checkListId, 
            int wgId, 
            string checklistName,
            List<SelectListItem> workingGroups
            //,CheckListserviceModel listOfServices
            )
        {
            this.CheckListId = checkListId;
            this.WGId = wgId;
            this.CheckListName = checklistName;
            this.WorkingGroups = workingGroups;
            //this.ListOfServices = listOfServices;
        }

               
        public int CheckListId { get; set; }

        public int? WGId { get; set; }

        public string CheckListName { get; set; }
             
        public List<SelectListItem> WorkingGroups {get; set;}

        //public CheckListserviceModel ListOfServices { get; set; }

        
    }
}