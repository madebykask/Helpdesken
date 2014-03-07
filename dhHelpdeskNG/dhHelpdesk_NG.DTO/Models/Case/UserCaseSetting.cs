using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class UserCaseSetting 
    {
        public UserCaseSetting(int customerId, int userId, string region, string registeredBy, bool caseType, 
                               string productArea, string workingGroup, bool responsible,
                               string administrators, string priority, bool state, string subState)
        {
            CustomerId = customerId;
            UserId = userId;
            Region = region;
            RegisteredBy = registeredBy;
            CaseType = caseType;
            ProductArea = productArea;
            WorkingGroup = workingGroup;
            Responsible = responsible;
            Administrators = administrators;
            Priority = priority;
            State = state;
            SubState = subState;
        }

        public int CustomerId { get; private set; }

        public int UserId { get; private set; }

        public string Region { get; private set; }

        public string RegisteredBy { get; private set; }

        public bool CaseType{ get; private set; }

        public string ProductArea { get; private set; }

        public string WorkingGroup { get; private set; }
        
        public bool Responsible{ get; private set; }

        public string Administrators { get; private set; }

        public string Priority { get; private set; }

        public bool State{ get; private set; }

        public string SubState { get; private set; }
        
        
    }
}
