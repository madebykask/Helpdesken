using System;
using System;
using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.SelfService.Case;

namespace DH.Helpdesk.SelfService.Models.Case
{
   
    public class CaseOverviewModel 
    {
        public CaseOverviewModel()
        { 

        }

        public CaseOverviewModel(int caseId, string notifier, string phone, string department, string pcNumber,
                                       DateTime registrationDate, string productArea, string watchDate, List<SelfServiceCaseLog>  caseLogs)
        {             
            caseId = caseId;
            Notifier = notifier;
            Phone = phone;
            Department = department;
            PCNumber = pcNumber;
            RegistrationDate = registrationDate;
            ProductArea = productArea;
            WatchDate = watchDate;                 
            CaseLogs = caseLogs;
        }
        
        
        public int caseId { get; set; }        

        public string Notifier { get; set; }

        public string Phone { get; set; }

        public string Department { get; set; }

        public string PCNumber { get; set; }        
        
        public DateTime RegistrationDate{ get; set; }

        public string ProductArea { get; set; }

        public string WatchDate { get; set; }

        public List<SelfServiceCaseLog> CaseLogs { get; set; }
        
    }
}
