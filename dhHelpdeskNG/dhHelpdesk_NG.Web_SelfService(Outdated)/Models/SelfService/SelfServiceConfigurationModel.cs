using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.SelfService.Models
{
    public class SelfServiceConfigurationModel
    {
        public bool ShowNewCase { get; set; }

        public bool ShowUserCases { get; set; }

        public bool ShowOrderAccount { get; set; }

        public bool ShowBulletinBoard { get; set; }

        public bool ShowFAQ { get; set; }

        public bool ShowDashboard { get; set; }

        public int ViewCaseMode { get; set; }

        public bool IsReceipt { get; set; }
    }
}