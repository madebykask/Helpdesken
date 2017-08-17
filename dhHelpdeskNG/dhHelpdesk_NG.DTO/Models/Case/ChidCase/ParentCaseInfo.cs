namespace DH.Helpdesk.BusinessData.Models.Case.ChidCase
{
    using System;

    public class ParentCaseInfo
    {
        /// <summary>
        /// Id of the parent case
        /// </summary>
        public int ParentId { get; set; }

        public decimal CaseNumber { get; set; }

        public UserNamesStruct CaseAdministrator { get; set; }

        public DateTime? FinishingDate { get; set; }
		public bool IsChildIndependent { get; internal set; }
	}
}