namespace DH.Helpdesk.Dal.Repositories.CaseDocument
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain.Cases;

    public interface ICaseDocumentRepository : IRepository<CaseDocumentEntity>
    {
        CaseDocumentModel GetCaseDocument(int id);

        IEnumerable<CaseDocumentModel> GetCaseDocumentsByCustomer(int customerId);
    }
}