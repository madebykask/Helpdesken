namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Invoice;

    public class CaseInvoiceArticleRepository : Repository, ICaseInvoiceArticleRepository
    {
        private readonly IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> invoiceToBusinessModelMapper;

        private readonly IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity> invoiceToEntityMapper;

        public CaseInvoiceArticleRepository(
                IDatabaseFactory databaseFactory, 
                IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> invoiceToBusinessModelMapper, 
                IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity> invoiceToEntityMapper)
            : base(databaseFactory)
        {
            this.invoiceToBusinessModelMapper = invoiceToBusinessModelMapper;
            this.invoiceToEntityMapper = invoiceToEntityMapper;
        }

        public CaseInvoice[] GetCaseInvoices(int caseId)
        {
            var entities = this.DbContext.CaseInvoices
                        .Where(i => i.CaseId == caseId)
                        .ToList();

            return entities.Select(i => this.invoiceToBusinessModelMapper.Map(i)).ToArray();
        }

        public void SaveCaseInvoices(IEnumerable<CaseInvoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                CaseInvoiceEntity entity;
                if (invoice.IsNew())
                {
                    entity = new CaseInvoiceEntity();
                    this.invoiceToEntityMapper.Map(invoice, entity);
                    this.DbContext.CaseInvoices.Add(entity);
                }
                else
                {
                    entity = this.DbContext.CaseInvoices.Find(invoice.Id);
                    this.invoiceToEntityMapper.Map(invoice, entity);
                }

                this.Commit();
            }            
        }

        public void DeleteCaseInvoices(int caseId)
        {
            var entities = this.DbContext.CaseInvoices
                        .Where(i => i.CaseId == caseId)
                        .ToList();

            foreach (var entity in entities)
            {
                this.DbContext.CaseInvoices.Remove(entity);
            }
        }
    }
}