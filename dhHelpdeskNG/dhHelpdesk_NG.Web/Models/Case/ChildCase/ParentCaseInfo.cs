namespace DH.Helpdesk.Web.Models.Case.ChildCase
{
    public class ParentCaseInfo
    {
        /// <summary>
        /// Id of the parent case
        /// </summary>
        public int ParentId;

        public decimal CaseNumber;

		public bool IsChildIndependent;

        public string Administrator;

        public bool IsCaseClosed;

        public bool RelationType;
    }
}