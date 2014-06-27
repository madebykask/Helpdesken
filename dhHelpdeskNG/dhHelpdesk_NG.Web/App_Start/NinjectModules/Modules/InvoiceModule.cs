namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel;
    using DH.Helpdesk.Domain.Invoice;

    using Ninject.Modules;

    public sealed class InvoiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBusinessModelToEntityMapper<InvoiceArticle, InvoiceArticleEntity>>()
                .To<InvoiceArticleToEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<InvoiceArticleUnit, InvoiceArticleUnitEntity>>()
                .To<InvoiceArticleUnitToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<InvoiceArticleEntity, InvoiceArticle>>()
                .To<InvoiceArticleToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<InvoiceArticleUnitEntity, InvoiceArticleUnit>>()
                .To<InvoiceArticleUnitToBusinessModelMapper>()
                .InSingletonScope();
        }
    }
}