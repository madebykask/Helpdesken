// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonModule.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CommonModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.ExternalInvoice;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.Common;
using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.Dal.Mappers.ExternalInvoice.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.ExternalInvoice.EntityToBusinessModel;
using DH.Helpdesk.Dal.Mappers.Gdpr.BusinessModelToEntity;
using DH.Helpdesk.Dal.Mappers.Gdpr.EntityToBusinessModel;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Web.Infrastructure.Cache;

namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.Customer.Input;
    using DH.Helpdesk.BusinessData.Models.Document.Output;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Infrastructure.Translate;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Calendars.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Calendars.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.Documents.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.ProductArea.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.ProductArea.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.CaseDocument;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common.Concrete;
    using DH.Helpdesk.Web.Infrastructure.Translate;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    using Ninject.Modules;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    using BusinessData.Models.Condition;
    using Dal.Mappers.Condition;

    /// <summary>
    /// The common module.
    /// </summary>
    public class CommonModule : NinjectModule
    {
        /// <summary>
        /// The load.
        /// </summary>
        public override void Load()
        {
            RegisterMappers();

            this.Bind<ICacheService>()
                .To<WebCacheService>();

            this.Bind<IHelpdeskCache>()
                .To<HelpdeskCache>();

            this.Bind<IModulesInfoFactory>().To<ModulesInfoFactory>().InSingletonScope();

            this.Bind<IDbQueryExecutorFactory>()
                .To<SqlDbQueryExecutorFactory>()
                .InSingletonScope();

            this.Bind<IJsonSerializeService>()
                .To<JsonSerializeService>()
                .InSingletonScope();
        }

        private void RegisterMappers()
        {
            this.Bind<IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>>()
                .To<ProductAreaToOverviewMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<ProductAreaOverview, ProductArea>>()
                .To<ProductAreaToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CausingPart, CausingPartOverview>>()
                .To<CausingPartToOverviewMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CausingPartOverview, CausingPart>>()
                .To<CausingPartToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Calendar, CalendarOverview>>()
                .To<CalendarToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CalendarOverview, Calendar>>()
                .To<CalendarToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .To<CustomerSettingsToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Document, DocumentOverview>>()
                .To<DocumentToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<ITranslator>().To<Translator>().InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Customer, CustomerOverview>>()
                .To<CustomerToBusinessModel>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<ExternalInvoice, CaseInvoiceRow>>()
                .To<ExternalInvoiceToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceRow, ExternalInvoice>>()
                .To<CaseInvoiceRowToBusinessModel>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower>>()
                .To<CaseExtraFollowersToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseSolutionConditionModel, CaseSolutionConditionEntity>>()
                .To<CaseSolutionConditionToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseSolutionConditionEntity, CaseSolutionConditionModel>>()
                .To<CaseSolutionConditionToBusinessModelMapper>()
                .InSingletonScope();


            this.Bind<IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity>>()
                .To<ExtendedCaseFormToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel>>()
                .To<ExtendedCaseFormToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity>>()
                .To<ExtendedCaseDataToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel>>()
                .To<ExtendedCaseDataToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseDocumentModel, CaseDocumentEntity>>()
                .To<CaseDocumentToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseDocumentEntity, CaseDocumentModel>>()
                .To<CaseDocumentToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseDocumentConditionModel, CaseDocumentConditionEntity>>()
                .To<CaseDocumentConditionToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseDocumentConditionEntity, CaseDocumentConditionModel>>()
                .To<CaseDocumentConditionToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<ExtendedCaseValueModel, ExtendedCaseValueEntity>>()
                .To<ExtendedCaseValueToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ExtendedCaseValueEntity, ExtendedCaseValueModel>>()
                .To<ExtendedCaseValueToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseDocumentParagraphModel, CaseDocumentParagraphEntity>>()
                .To<CaseDocumentParagraphToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseDocumentParagraphEntity, CaseDocumentParagraphModel>>()
                .To<CaseDocumentParagraphToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseDocumentTextConditionModel, CaseDocumentTextConditionEntity>>()
                .To<CaseDocumentTextConditionToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseDocumentTextConditionEntity, CaseDocumentTextConditionModel>>()
                .To<CaseDocumentTextConditionToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseDocumentTemplateModel, CaseDocumentTemplateEntity>>()
                .To<CaseDocumentTemplateToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseDocumentTemplateEntity, CaseDocumentTemplateModel>>()
                .To<CaseDocumentTemplateToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseDocumentTextIdentifierModel, CaseDocumentTextIdentifierEntity>>()
                .To<CaseDocumentTextIdentifierToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseDocumentTextIdentifierEntity, CaseDocumentTextIdentifierModel>>()
                .To<CaseDocumentTextIdentifierToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseDocumentTextConditionIdentifierModel, CaseDocumentTextConditionIdentifierEntity>>()
                .To<CaseDocumentTextConditionIdentifierToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseDocumentTextConditionIdentifierEntity, CaseDocumentTextConditionIdentifierModel>>()
                .To<CaseDocumentTextConditionIdentifierToBusinessModelMapper>()
                .InSingletonScope();


            this.Bind<IBusinessModelToEntityMapper<ConditionModel, ConditionEntity>>()
                .To<ConditionToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ConditionEntity, ConditionModel>>()
                .To<ConditionToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<GdprFavoriteModel, GDPRDataPrivacyFavorite>>()
                .To<GdprFavoriteModelToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<GDPRDataPrivacyFavorite, GdprFavoriteModel>>()
                .To<GdprFavoriteEntityToModelMapper>()
                .InSingletonScope();
            
        }
    }
}