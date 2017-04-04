using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Enums.Case;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseIntLog
{
    public class CaseEmailSendOverview
    {

        public CaseEmailSendOverview()
        {
            UserId = string.Empty;
        }
        public string UserId { get; set; }

        public string Name { get; set; }

        public List<string> Emails { get; set; }

        public string DepartmentName { get; set; }

        public CaseUserSearchGroup GroupType { get; set; }
    }
}
