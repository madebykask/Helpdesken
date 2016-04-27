namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Invoice;
    using System.Collections.ObjectModel;

    public sealed class CaseInvoiceToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>
    {
        private readonly IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper;

        private readonly IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper;

        private readonly IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile> filesMapper;

        public CaseInvoiceToBusinessModelMapper(
            IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper, 
            IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper, 
            IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile> filesMapper)
        {
            this.caseMapper = caseMapper;
            this.articleMapper = articleMapper;
            this.filesMapper = filesMapper;
        }

        public CaseInvoice Map(CaseInvoiceEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            if (entity.Orders != null)
            {
                foreach (var order in entity.Orders)
                {
                    if (order.Articles == null)
                        order.Articles = new Collection<CaseInvoiceArticleEntity>();

                    if (order.Files == null)
                        order.Files = new Collection<CaseInvoiceOrderFileEntity>();
                }
            }

            return new CaseInvoice(
                    entity.Id,
                    entity.CaseId,
                    this.caseMapper.Map(entity.Case),
                    entity.Orders
                        .Select(o => new CaseInvoiceOrder(
                                o.Id, 
                                o.InvoiceId, 
                                null, 
                                o.Number, 
                                o.InvoiceDate,
                                o.InvoicedByUserId,
                                o.Date,
                                o.ReportedBy,
                                o.Persons_Name,
                                o.Persons_Email,
                                o.Persons_Phone,
                                o.Persons_Cellphone,
                                o.Region_Id,
                                o.Department_Id,
                                o.OU_Id,
                                o.Place,
                                o.UserCode,
                                o.CostCentre,
                                o.CreditForOrder_Id,
                                o.Project_Id,
                                o.Articles != null? 
                                    o.Articles.Select(a => new CaseInvoiceArticle(
                                                    a.Id,
                                                    a.OrderId,
                                                    null,
                                                    a.ArticleId,
                                                    this.articleMapper.Map(a.Article),
                                                    a.Name,
                                                    a.Amount,
                                                    a.Ppu,
                                                    a.Position,
                                                    a.CreditedForArticle_Id)).ToArray():null,
                                 o.Articles != null ?
                                    o.Files.Select(f => this.filesMapper.Map(f)).OrderBy(f => f.FileName).ToArray() : null
                                 ))
                                .OrderBy(o => o.Number).ToArray());
        }
    }
}