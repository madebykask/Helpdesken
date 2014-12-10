using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class workingGroupMapper
    {
     
        public workingGroupMapper(int id, string name)
        {
            // TODO: Complete member initialization
            this.workingGroupId = id;
            this.WorkingGroupName = name;
        }
        public int? workingGroupId { get; set; }
        public string WorkingGroupName { get; set; }
        
    }
}