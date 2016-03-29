using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.WorkingGroup
{
    public class WorkingGroupForSMS
    {
        public WorkingGroupForSMS(int id, string workingGroupName, string phoneNumbers)
        {
            Id = id;
            WorkingGroupName = workingGroupName;
            PhoneNumbers = phoneNumbers;
        }

        public int Id { get; set; }

        public string WorkingGroupName { get; set; }

        public string PhoneNumbers { get; set; } 

    }
}
