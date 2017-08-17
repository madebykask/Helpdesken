namespace DH.Helpdesk.Dal.Repositories.CaseDocument
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    
    public interface ICaseDocumentConditionRepository : IRepository<CaseDocumentConditionEntity>
    {
      
        IEnumerable<CaseDocumentConditionModel> GetCaseDocumentConditions(int caseDocumentId);
    }
}