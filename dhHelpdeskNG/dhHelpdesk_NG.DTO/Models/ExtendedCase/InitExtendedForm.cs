using DH.Helpdesk.BusinessData.Models.Case;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
	public class InitExtendedForm
	{
        public InitExtendedForm(int customerId, int languageId, string userName,
                                int? caseSolutionId, int? caseId)
        {
            CustomerId = customerId;
            LanguageId = languageId;
            UserName = userName;
            CaseSolutionId = caseSolutionId;            
            CaseId = caseId.HasValue? caseId.Value : 0;
        }

        public int CustomerId { get; private set; }

        public int LanguageId { get; private set; }

        public string UserName { get; private set; }

        public int? CaseSolutionId { get; private set; }        

        public int CaseId { get; private set; }


    }
}
