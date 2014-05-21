﻿namespace DH.Helpdesk.Web.NinjectModules.Common
{
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;

    using Ninject.Modules;

    public sealed class ServicesModule : NinjectModule
    {
        #region Public Methods and Operators

        public override void Load()
        {
            this.Bind<IChangeService>().To<ChangeService>();
            this.Bind<IFaqService>().To<FaqService>();
            this.Bind<ILinkService>().To<LinkService>();
            this.Bind<INotifierService>().To<NotifierService>();
            this.Bind<IProblemService>().To<ProblemService>();
            this.Bind<IProjectService>().To<ProjectService>();

            this.Bind<IChangeCategoryService>().To<ChangeCategoryService>();
            this.Bind<IChangeGroupService>().To<ChangeGroupService>();
            this.Bind<IChangeImplementationStatusService>().To<ChangeImplementationStatusService>();
            this.Bind<IChangeObjectService>().To<ChangeObjectService>();
            this.Bind<IChangePriorityService>().To<ChangePriorityService>();
            this.Bind<IChangeStatusService>().To<ChangeStatusService>();
            this.Bind<IQestionnaireService>().To<QuestionnaireService>();
            this.Bind<IQestionnaireQuestionService>().To<QuestionnaireQuestionService>();
            this.Bind<IQestionnaireQuestionOptionService>().To<QuestionnaireQuestionOptionService>();
            this.Bind<ICircularService>().To<CircularService>();
            this.Bind<IInventoryService>().To<InventoryService>();
            this.Bind<IAccountService>().To<AccountService>();
            this.Bind<IAccountActivityService>().To<AccountActivityService>();
            this.Bind<IAccountFieldSettingsService>().To<AccountFieldSettingsService>();
            this.Bind<IBuildingService>().To<BuildingService>();
            this.Bind<IBulletinBoardService>().To<BulletinBoardService>();
            this.Bind<ICalendarService>().To<CalendarService>();
            this.Bind<ICaseFieldSettingService>().To<CaseFieldSettingService>();
            this.Bind<ICaseService>().To<CaseService>();
            this.Bind<ICaseSettingsService>().To<CaseSettingsService>();
            this.Bind<ICaseSolutionService>().To<CaseSolutionService>();
            this.Bind<ICaseFileService>().To<CaseFileService>();
            this.Bind<ICaseTypeService>().To<CaseTypeService>();
            this.Bind<ICaseSearchService>().To<CaseSearchService>();
            this.Bind<ICategoryService>().To<CategoryService>();
            this.Bind<IChecklistActionService>().To<ChecklistActionService>();
            this.Bind<IChecklistServiceService>().To<ChecklistServiceService>();
            this.Bind<IComputerService>().To<ComputerService>();
            this.Bind<IContractCategoryService>().To<ContractCategoryService>();
            this.Bind<ICountryService>().To<CountryService>();
            this.Bind<ICustomerService>().To<CustomerService>();
            this.Bind<ICurrencyService>().To<CurrencyService>();
            this.Bind<ICustomerUserService>().To<CustomerUserService>();
            this.Bind<IDailyReportService>().To<DailyReportService>();
            this.Bind<IDepartmentService>().To<DepartmentService>();
            this.Bind<IDivisionService>().To<DivisionService>();
            this.Bind<IDocumentService>().To<DocumentService>();
            this.Bind<IDomainService>().To<DomainService>();
            this.Bind<IEmailGroupService>().To<EmailGroupService>();
            this.Bind<IFormService>().To<FormService>();
            this.Bind<IFinishingCauseService>().To<FinishingCauseService>();
            this.Bind<IFloorService>().To<FloorService>();
            this.Bind<IGlobalSettingService>().To<GlobalSettingService>();
            this.Bind<IHolidayService>().To<HolidayService>();
            this.Bind<IImpactService>().To<ImpactService>();
            this.Bind<IInfoService>().To<InfoService>();
            this.Bind<ILanguageService>().To<LanguageService>();
            this.Bind<ILogFileService>().To<LogFileService>();
            this.Bind<ILogService>().To<LogService>();
            this.Bind<IMailTemplateService>().To<MailTemplateService>();
            this.Bind<IMasterDataService>().To<MasterDataService>();
            this.Bind<IOperationLogCategoryService>().To<OperationLogCategoryService>();
            this.Bind<IOperationObjectService>().To<OperationObjectService>();
            this.Bind<IOrderService>().To<OrderService>();
            this.Bind<IOperationLogService>().To<OperationLogService>();
            this.Bind<IOrderStateService>().To<OrderStateService>();
            this.Bind<IOrderTypeService>().To<OrderTypeService>();
            this.Bind<IOUService>().To<OUService>();
            this.Bind<IPriorityService>().To<PriorityService>();
            this.Bind<IProblemLogService>().To<ProblemLogService>();
            this.Bind<IProductAreaService>().To<ProductAreaService>();
            this.Bind<IProgramService>().To<ProgramService>();
            this.Bind<IRegionService>().To<RegionService>();
            this.Bind<IRoomService>().To<RoomService>();
            this.Bind<ISettingService>().To<SettingService>();
            this.Bind<IStandardTextService>().To<StandardTextService>();
            this.Bind<IStateSecondaryService>().To<StateSecondaryService>();
            this.Bind<IStatusService>().To<StatusService>();
            this.Bind<IStatisticsService>().To<StatisticsService>();
            this.Bind<ISupplierService>().To<SupplierService>();
            this.Bind<ISystemService>().To<SystemService>();
            this.Bind<ITemplateService>().To<TemplateService>();
            this.Bind<ITextTranslationService>().To<TextTranslationService>();
            this.Bind<IUrgencyService>().To<UrgencyService>();
            this.Bind<IUserService>().To<UserService>();
            this.Bind<IWatchDateCalendarService>().To<WatchDateCalendarService>();
            this.Bind<IWorkingGroupService>().To<WorkingGroupService>();
            this.Bind<ICausingPartService>().To<CausingPartService>();
            this.Bind<IEmailService>().To<EmailService>().InSingletonScope();
            this.Bind<IReportsService>().To<ReportsService>();
            this.Bind<IComputerModulesService>().To<ComputerModulesService>();
        }

        #endregion
    }
}