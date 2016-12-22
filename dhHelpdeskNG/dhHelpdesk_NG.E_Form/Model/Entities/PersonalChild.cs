using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.Model.Entities
{
    public class FamilyMember 
    {
        public DynamicFields MemberFirstname { get; set; }
        public DynamicFields MemberLastname { get; set; }
        public DynamicFields MemberPESEL { get; set; }
        public DynamicFields Healthinsurance { get; set; }
        public DynamicFields Joinedhousehold { get; set; }
        public DynamicFields Exclusivelydependant { get; set; }
    }
}
