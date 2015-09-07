namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceOrderToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceOrderEntity, CaseInvoiceOrder>
    {
        private readonly IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> caseInvoiceMapper;

        private readonly IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle> caseArticleMapper;

        private readonly IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile> filesMapper;

        public CaseInvoiceOrderToBusinessModelMapper(
            IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice> caseInvoiceMapper, 
            IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle> caseArticleMapper, 
            IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile> filesMapper)
        {
            this.caseInvoiceMapper = caseInvoiceMapper;
            this.caseArticleMapper = caseArticleMapper;
            this.filesMapper = filesMapper;
        }

        public CaseInvoiceOrder Map(CaseInvoiceOrderEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CaseInvoiceOrder(
                        entity.Id,
                        entity.InvoiceId,
                        this.caseInvoiceMapper.Map(entity.Invoice),
                        entity.Number,
                        entity.DeliveryPeriod,
                        entity.InvoiceDate,
                        entity.InvoicedByUserId,
                        entity.Reference,
                        entity.Date,
                        entity.ReportedBy,
                        entity.Persons_Name,
                        entity.Persons_Phone,
                        entity.Persons_Cellphone,
                        entity.Region_Id,
                        entity.Department_Id,
                        entity.OU_Id,
                        entity.Place,
                        entity.UserCode,
                        entity.CostCentre,
                        entity.Articles.Select(a => this.caseArticleMapper.Map(a)).OrderBy(a => a.Position).ToArray(),
                        entity.Files.Select(f => this.filesMapper.Map(f)).OrderBy(f => f.FileName).ToArray());
        }
    }
}