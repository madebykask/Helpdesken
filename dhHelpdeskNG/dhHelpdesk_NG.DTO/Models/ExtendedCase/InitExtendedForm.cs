using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
	public class InitExtendedForm
	{
	    public InitExtendedForm(int customerId, int languageId, string userName,
                                int? caseSolutionId, int? caseId, string userRole, int caseStateSecondaryId)
        {
            CustomerId = customerId;
            LanguageId = languageId;
            UserName = userName;
            CaseSolutionId = caseSolutionId;            
            CaseId = caseId ?? 0;
            UserRole = userRole;
            CaseStateSecondaryId = caseStateSecondaryId;
        }

        public int CustomerId { get; private set; }

        public int LanguageId { get; private set; }

        public string UserName { get; private set; }

        public int? CaseSolutionId { get; private set; }        

        public int CaseId { get; private set; }

        public string UserRole { get; private set; }

        public int CaseStateSecondaryId { get; private set; }
    }
}
