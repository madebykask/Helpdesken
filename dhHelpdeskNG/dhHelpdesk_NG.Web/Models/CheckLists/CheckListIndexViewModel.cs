using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DH.Helpdesk.Web.Models.CheckLists
{    

    public class CheckListIndexViewModel
    {

        public CheckListIndexViewModel()
        { }

        public List<SelectListItem> WorkingGroups;
        public List<SelectListItem> ListOfExistances;
        public DateTime From;
        public DateTime To;
        public List<ChecklistInputModel> CheckListsList;
        public List<SelectListItem> Show;


    }
}