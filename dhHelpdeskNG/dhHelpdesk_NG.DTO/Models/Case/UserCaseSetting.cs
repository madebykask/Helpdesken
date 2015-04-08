namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    public class UserCaseSetting 
    {
        public UserCaseSetting(
                    int customerId, 
                    int userId, 
                    string region, 
                    string registeredBy, 
                    string caseType, 
                    string productArea, 
                    string workingGroup, 
                    bool responsible,
                    string administrators, 
                    string priority, 
                    bool state, 
                    string subState, 
                    DateTime? caseRegistrationDateStartFilter, 
                    DateTime? caseRegistrationDateEndFilter, 
                    DateTime? caseWatchDateStartFilter, 
                    DateTime? caseWatchDateEndFilter, 
                    DateTime? caseClosingDateStartFilter, 
                    DateTime? caseClosingDateEndFilter, 
                    bool caseRegistrationDateFilterShow, 
                    bool caseWatchDateFilterShow, 
                    bool caseClosingDateFilterShow, 
                    string caseClosingReasonFilter,
                    bool caseInitiatorFilterShow)
        {
            this.CaseClosingReasonFilter = caseClosingReasonFilter;
            this.CaseClosingDateFilterShow = caseClosingDateFilterShow;
            this.CaseWatchDateFilterShow = caseWatchDateFilterShow;
            this.CaseRegistrationDateFilterShow = caseRegistrationDateFilterShow;
            this.CaseClosingDateEndFilter = caseClosingDateEndFilter;
            this.CaseClosingDateStartFilter = caseClosingDateStartFilter;
            this.CaseWatchDateEndFilter = caseWatchDateEndFilter;
            this.CaseWatchDateStartFilter = caseWatchDateStartFilter;
            this.CaseRegistrationDateEndFilter = caseRegistrationDateEndFilter;
            this.CaseRegistrationDateStartFilter = caseRegistrationDateStartFilter;
            this.CustomerId = customerId;
            this.UserId = userId;
            this.Region = region;
            this.RegisteredBy = registeredBy;
            this.CaseType = caseType;
            this.ProductArea = productArea;
            this.WorkingGroup = workingGroup;
            this.Responsible = responsible;
            this.Administrators = administrators;
            this.Priority = priority;
            this.State = state;
            this.SubState = subState;
            this.CaseInitiatorFilterShow = caseInitiatorFilterShow;
        }

        public int CustomerId { get; private set; }

        public int UserId { get; private set; }

        public string Region { get; private set; }

        public string RegisteredBy { get; private set; }

        public string CaseType { get; private set; }

        public string ProductArea { get; private set; }

        public string WorkingGroup { get; private set; }
        
        public bool Responsible { get; private set; }

        public string Administrators { get; private set; }

        public string Priority { get; private set; }

        public bool State { get; private set; }

        public string SubState { get; private set; }

        public DateTime? CaseRegistrationDateStartFilter { get; private set; }

        public DateTime? CaseRegistrationDateEndFilter { get; private set; }

        public DateTime? CaseWatchDateStartFilter { get; private set; }

        public DateTime? CaseWatchDateEndFilter { get; private set; }

        public DateTime? CaseClosingDateStartFilter { get; private set; }

        public DateTime? CaseClosingDateEndFilter { get; private set; }

        public bool CaseRegistrationDateFilterShow { get; private set; }

        public bool CaseWatchDateFilterShow { get; private set; }

        public bool CaseClosingDateFilterShow { get; private set; }

        public string CaseClosingReasonFilter { get; private set; }

        /// <summary>
        /// Flag to display "filter by intitator" field on case overview page
        /// </summary>
        public bool CaseInitiatorFilterShow { get; private set; }
    }
}
