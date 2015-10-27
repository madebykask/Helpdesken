namespace DH.Helpdesk.Web.Models.Case.ChildCase
{
    public class ParentCaseInfo
    {
        /// <summary>
        /// Id of the parent case
        /// </summary>
        public int ParentId;

        public decimal CaseNumber;

        public string Administrator;

        public bool IsCaseClosed;
    }
}