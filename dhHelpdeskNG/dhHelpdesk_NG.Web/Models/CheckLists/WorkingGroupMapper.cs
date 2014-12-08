using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class workingGroupMapper
    {
        private int p1;
        private string p2;

        public workingGroupMapper(int p1, string p2)
        {
            // TODO: Complete member initialization
            this.p1 = p1;
            this.p2 = p2;
        }
        public int? workingGroupId { get; set; }
        public string WorkingGroupName { get; set; }
        
    }
}