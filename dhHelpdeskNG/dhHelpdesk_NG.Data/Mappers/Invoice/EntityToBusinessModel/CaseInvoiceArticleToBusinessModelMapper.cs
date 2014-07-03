namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class CaseInvoiceArticleToBusinessModelMapper : IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle>
    {
        private readonly IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper;

        private readonly IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper;

        public CaseInvoiceArticleToBusinessModelMapper(
            IEntityToBusinessModelMapper<Case, CaseOverview> caseMapper, 
            IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle> articleMapper)
        {
            this.caseMapper = caseMapper;
            this.articleMapper = articleMapper;
        }

        public CaseInvoiceArticle Map(CaseInvoiceArticleEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CaseInvoiceArticle(
                        entity.Id,
                        entity.CaseId,
                        this.caseMapper.Map(entity.Case),
                        entity.ArticleId,
                        this.articleMapper.Map(entity.Article),
                        entity.Name,
                        entity.Amount,
                        entity.Position,
                        entity.IsInvoiced);
        }
    }
}