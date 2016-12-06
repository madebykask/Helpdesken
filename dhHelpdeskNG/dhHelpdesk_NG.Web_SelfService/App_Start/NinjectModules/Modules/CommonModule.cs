﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonModule.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CommonModule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.SelfService.NinjectModules.Modules
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.BusinessData.Models.Case.Input;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.BusinessData.Models.Customer.Input;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.BusinessData.Models.Projects.Input;
    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Dal.Infrastructure.Translate;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.Invoice.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.Invoice.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.ProductArea.BusinessModelToEntity;
    using DH.Helpdesk.Dal.Mappers.ProductArea.EntityToBusinessModel;
    using DH.Helpdesk.Dal.Mappers.Projects;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Domain.Invoice;
    using DH.Helpdesk.Domain.Projects;
    using DH.Helpdesk.SelfService.Infrastructure.Translate;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users.Concrete;
    using Ninject.Modules;

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

            this.Bind<IUserPermissionsChecker>().To<UserPermissionsChecker>();

            this.Bind<IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>>()
                .To<ProductAreaToOverviewMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<ProductAreaOverview, ProductArea>>()
                .To<ProductAreaToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Setting, CustomerSettings>>()
                .To<CustomerSettingsToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseNotifier, ComputerUser>>()
                .To<CaseNotifierToEntityMapper>()
                .InSingletonScope();

            this.Bind<ITranslator>().To<Translator>().InSingletonScope();

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

            this.Bind<IBusinessModelToEntityMapper<CaseInvoiceArticle, CaseInvoiceArticleEntity>>()
                .To<CaseInvoiceArticleToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Case, CaseOverview>>()
               .To<CaseToBusinessModelMapper>()
               .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Customer, CustomerOverview>>()
               .To<CustomerToBusinessModel>()
               .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceEntity, CaseInvoice>>()
               .To<CaseInvoiceToBusinessModelMapper>()               
               .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoice, CaseInvoiceEntity>>()
                .To<CaseInvoiceToEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoiceOrder, CaseInvoiceOrderEntity>>()
                .To<CaseInvoiceOrderToEntityMapper>()
                .InSingletonScope();


            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceOrderFileEntity, CaseInvoiceOrderFile>>()
                .To<CaseInvoiceOrderFileToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoiceOrderFile, CaseInvoiceOrderFileEntity>>()
                .To<CaseInvoiceOrderFileToEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseLock, CaseLockEntity>>()
             .To<CaseLockToEntityMapper>()
             .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseLockEntity, CaseLock>>()
                .To<CaseLockToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseFilterFavorite, CaseFilterFavoriteEntity>>()
                .To<CaseFilterFavoritToEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite>>()
                .To<CaseFilterFavoriteToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseInvoiceSettingsEntity, CaseInvoiceSettings>>()
                .To<CaseInvoiceSettingsToBusinessModelMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<CaseInvoiceSettings, CaseInvoiceSettingsEntity>>()
                .To<CaseInvoiceSettingsToEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProject, Project>>()
               .To<NewProjectToProjectEntityMapper>()
               .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectCollaborator, ProjectCollaborator>>()
               .To<NewProjectCollaboratorlToProjectCollaboratorEntityMapper>()
               .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile>>()
                .To<NewProjectFileToProjectFileEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectSchedule, ProjectSchedule>>()
                .To<NewProjectScheduleToProjectScheduleEntityMapper>()
                .InSingletonScope();

            this.Bind<INewBusinessModelToEntityMapper<NewProjectLog, ProjectLog>>()
                .To<NewProjectLogToProjectLogEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<UpdatedProject, Project>>()
                .To<UpdatedProjectToProjectEntityMapper>()
                .InSingletonScope();

            this.Bind<IBusinessModelToEntityMapper<UpdatedProjectSchedule, ProjectSchedule>>()
                .To<UpdatedProjectScheduleToProjectScheduleEntityMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<Project, ProjectOverview>>()
                .To<ProjectEntityToProjectOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectCollaborator, ProjectCollaboratorOverview>>()
                .To<ProjectCollaboratorEntityToNewProjectCollaboratorMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview>>()
                .To<ProjectFileEntityToProjectFileOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectLog, ProjectLogOverview>>()
                .To<ProjectLogEntityToProjectLogOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<ProjectSchedule, ProjectScheduleOverview>>()
                .To<ProjectScheduleEntityToProjectScheduleOverviewMapper>()
                .InSingletonScope();

            this.Bind<IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower>>()
                .To<CaseExtraFollowersToBusinessModelMapper>()
                .InSingletonScope();
        }
    }
}