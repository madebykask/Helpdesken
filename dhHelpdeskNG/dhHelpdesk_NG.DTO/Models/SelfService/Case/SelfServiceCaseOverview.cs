using System;
using System.Collections.Generic;

using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.SelfService.Case
{

    public class SelfServiceCaseLog
    {
        public SelfServiceCaseLog(int logId, DateTime logDate, string textExternal, string textInternal )
        {
            LogId = logId;
            LogDate = logDate;
            TextExternal = textExternal;
            TextIntenal = textInternal;
        }

        [IsId]
        public int LogId { get; private set;}

        public DateTime LogDate { get; private set;}

        public string TextExternal {get; private set;}

        public string TextIntenal {get; private set;}

    }

    public class SelfServiceCaseOverview 
    {
        public SelfServiceCaseOverview(int caseId, string personName, string personPhone, string department, string pcNumber,
                                       DateTime registrationDate, string productArea, string watchDate, List<SelfServiceCaseLog>  caseLogs)
        {
             
            this.caseId = caseId;
            PersonName = personName;
            PersonPhone = personPhone;
            Department = department;
            PCNumber = pcNumber;            
            RegistrationDate = registrationDate;
            ProductArea = productArea;
            WatchDate = watchDate;                 
            CaseLogs = caseLogs;
        }
        
        [IsId]
        public int caseId { get; private set; }

        public string PersonName { get; private set; }

        public string PersonPhone { get; private set; }

        public string Department { get; private set; }

        public string PCNumber { get; private set; }        
        
        public DateTime RegistrationDate{ get; private set; }

        public string ProductArea { get; private set; }

        public string WatchDate { get; private set; }

        public List<SelfServiceCaseLog> CaseLogs { get; private set; }
        
    }
}
