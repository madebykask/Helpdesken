namespace DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Customer.Input;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class InvoiceArticleToBusinessModelMapper : IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle>
    {
        private readonly IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit> unitMapper;

        private readonly IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview> productAreaMapper;

        private readonly IEntityToBusinessModelMapper<Customer, CustomerOverview> customerMapper;

        public InvoiceArticleToBusinessModelMapper(
            IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit> unitMapper, 
            IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview> productAreaMapper, 
            IEntityToBusinessModelMapper<Customer, CustomerOverview> customerMapper)
        {
            this.unitMapper = unitMapper;
            this.productAreaMapper = productAreaMapper;
            this.customerMapper = customerMapper;
        }

        public InvoiceArticle Map(InvoiceArticleEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new InvoiceArticle(
                        entity.Id,
                        entity.ParentId,
                        entity.Parent != null ? this.Map(entity.Parent) : null,
                        entity.Number,
                        entity.Name,
                        entity.NameEng,
                        entity.Description,
                        entity.UnitId,
                        this.unitMapper.Map(entity.Unit),
                        entity.Ppu,
                        entity.ProductAreaId,
                        this.productAreaMapper.Map(entity.ProductArea),
                        entity.CustomerId,
                        this.customerMapper.Map(entity.Customer));
        }
    }
}