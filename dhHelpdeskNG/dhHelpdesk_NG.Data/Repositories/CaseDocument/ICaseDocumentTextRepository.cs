namespace DH.Helpdesk.Dal.Repositories.CaseDocument
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    
    public interface ICaseDocumentTextConditionRepository : IRepository<CaseDocumentTextConditionEntity>
    {
      
        IEnumerable<CaseDocumentTextConditionModel> GetCaseDocumentTextConditions(int caseDocumentId);
    }
}