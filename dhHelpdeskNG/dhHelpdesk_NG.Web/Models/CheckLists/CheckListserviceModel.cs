using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class CheckListserviceModel
    {
        public int Customer_Id { get; set; }
        public int Service_Id { get; set; }
        public int IsActive { get; set; }
        public string ServiceName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public ServicesActionsModel ListOfActions { get; set; }

    }
}