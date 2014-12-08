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

        public ChecklistInputModel(int checkListId, int wgId, string checklistName, List<workingGroupMapper> workingGroup,
                              DateTime createdDate, DateTime changedDate, CheckListserviceModel listOfServices)
        {
            // TODO: Complete member initialization
            this.CheckListId = checkListId;
            this.WGId = wgId;
            this.CheckListName = checklistName;
            this.WorkingGroups = workingGroup.Select(x => new SelectListItem
                {
                    Selected = (x.workingGroupId == wgId ? true : false),
                    Text = x.WorkingGroupName,
                    Value = x.workingGroupId.ToString()
                }).ToList();  ;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.ListOfServices = listOfServices;
        }

               
        public int CheckListId { get; set; }

        public int? WGId { get; set; }

        public  string CheckListName { get; set; }
             
        public List<SelectListItem> WorkingGroups {get; set;}

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        //public List<SelectListItem> Show { get; set; }

        public CheckListserviceModel ListOfServices { get; set; }

        
    }
}