namespace DH.Helpdesk.Dal.Repositories.CaseDocument
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    
    public interface ICaseDocumentTextConditionIdentifierRepository : IRepository<CaseDocumentTextConditionIdentifierEntity>
    {
      
        IEnumerable<CaseDocumentTextConditionIdentifierModel> GetCaseDocumentTextConditionIdentifiers(int extendedCaseFormId);


        CaseDocumentTextConditionIdentifierModel GetCaseDocumentTextConditionPropertyName(int extendedCaseFormId, string identifier);
    }
}