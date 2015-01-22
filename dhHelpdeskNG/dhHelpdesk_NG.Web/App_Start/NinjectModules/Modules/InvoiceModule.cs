namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel;
    using DH.Helpdesk.Domain.Invoice;
    using DH.Helpdesk.Web.Areas.Admin.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Invoice.Concrete;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;

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

            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceArticleEntity, CaseInvoiceArticle>>()
                .To<CaseInvoiceArticleToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceOrderEntity, CaseInvoiceOrder>>()
                .To<CaseInvoiceOrderToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile>>()
                .To<CaseInvoiceOrderFileToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>>()
                .To<CaseInvoiceToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity>>()
                .To<CaseInvoiceArticleToEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity>>()
                .To<CaseInvoiceToEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>>()
                .To<CaseInvoiceOrderToEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity>>()
                .To<CaseInvoiceOrderFileToEntityMapper>()
                .InSingletonScope();

            this.Bind<IInvoiceArticlesModelFactory>()
                .To<InvoiceArticlesModelFactory>()
                .InSingletonScope();

            this.Bind<ICaseInvoiceFactory>()
                .To<CaseInvoiceFactory>()
                .InSingletonScope();

            this.Bind<IInvoiceHelper>()
                .To<InvoiceHelper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings>>()
                .To<CaseInvoiceSettingsToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity>>()
                .To<CaseInvoiceSettingsToEntityMapper>()
                .InSingletonScope();
        }
    }
}