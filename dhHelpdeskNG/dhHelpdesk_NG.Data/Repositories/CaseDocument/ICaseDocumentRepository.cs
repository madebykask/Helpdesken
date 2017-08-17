namespace DH.Helpdesk.Dal.Repositories.CaseDocument
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    using System;

    public interface ICaseDocumentRepository : IRepository<CaseDocumentEntity>
    {
        CaseDocumentModel GetCaseDocument(Guid caseDocumentGUID);

        IEnumerable<CaseDocumentModel> GetCaseDocumentsByCustomer(int customerId);
    }
}