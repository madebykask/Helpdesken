using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler.Dto
{
    internal class CompuerUsersFieldSetting
    {
        public CompuerUsersFieldSetting()
        {

        }
            public int Id { get; set; }

            public int Customer_Id { get; set; }           

            public string ComputerUserField { get; set; }                  

            public string LDAPAttribute { get; set; }

            public int MinLength { get; set; }

            public int Required { get; set; }

            public int Show { get; set; }

            public int ShowInList { get; set; }

            public DateTime CreatedDate { get; set; }

            public DateTime ChangedDate { get; set; }
    }
}
