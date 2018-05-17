namespace DH.Helpdesk.Dal.Repositories.CaseDocument
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    using System;

    public interface ICaseDocumentRepository : IRepository<CaseDocumentEntity>
    {
        CaseDocumentModel GetCaseDocumentFull(Guid caseDocumentGUID);
        IList<CaseDocumentOverview> GetCustomerCaseDocumentsWithConditions(int caseId, int customerId);
    }
}