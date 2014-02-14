﻿namespace DH.Helpdesk.Dal.DbContext
{
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Text;

    using DH.Helpdesk.Dal.EntityConfigurations;
    using DH.Helpdesk.Dal.EntityConfigurations.Changes;
    using DH.Helpdesk.Dal.EntityConfigurations.Computers;
    using DH.Helpdesk.Dal.EntityConfigurations.Faq;
    using DH.Helpdesk.Dal.EntityConfigurations.Printers;
    using DH.Helpdesk.Dal.EntityConfigurations.Problems;
    using DH.Helpdesk.Dal.EntityConfigurations.Projects;
    using DH.Helpdesk.Dal.EntityConfigurations.Questionnaire;
    using DH.Helpdesk.Dal.EntityConfigurations.Servers;
    using DH.Helpdesk.Dal.EntityConfigurations.WorkstationModules;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Domain.Inventory;
    using DH.Helpdesk.Domain.Printers;
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.Domain.Projects;
    using DH.Helpdesk.Domain.Servers;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Domain.WorkstationModules;

    using OperatingSystemConfiguration = DH.Helpdesk.Dal.EntityConfigurations.OperatingSystemConfiguration;

    public class HelpdeskDbContext : DbContext
    {
        public HelpdeskDbContext()
            : base()
        {
        }

        public HelpdeskDbContext(DbConnection connection)
            : base(connection, true)
        {
        }

        #region public DbSet

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountActivity> AccountActivities { get; set; }
        public DbSet<AccountActivityGroup> AccountActivityGroups { get; set; }
        public DbSet<AccountEMailLog> AccountEMailLogs { get; set; }
        public DbSet<AccountFieldSettings> AccountFieldSettings { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<BulletinBoard> BulletinBoards { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CaseSettings> CaseSettings { get; set; }
        public DbSet<CaseFieldSetting> CaseFieldSettings { get; set; }
        public DbSet<CaseFieldSettingLanguage> CaseFieldSettingLanguages { get; set; }
        public DbSet<CaseFile> CaseFiles { get; set; }
        public DbSet<CaseHistory> CaseHistories { get; set; }
        public DbSet<CaseInvoiceRow> CaseInvoiceRows { get; set; }
        public DbSet<CaseQuestion> CaseQuestions { get; set; }
        public DbSet<CaseQuestionCategory> CaseQuestionCategories { get; set; }
        public DbSet<CaseQuestionHeader> CaseQuestionHeaders { get; set; }
        public DbSet<CaseSolutionCategory> CaseSolutionCategories { get; set; }
        public DbSet<CaseSolution> CaseSolutions { get; set; }
        public DbSet<CaseSolutionSchedule> CaseSolutionSchedules { get; set; }
        public DbSet<CaseType> CaseTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ChangeEntity> Changes { get; set; }
        public DbSet<ChangeCategoryEntity> ChangeCategories { get; set; }
        public DbSet<ChangeEmailLogEntity> ChangeEMailLogs { get; set; }
        public DbSet<ChangeFieldSettingsEntity> ChangeFieldSettings { get; set; }
        public DbSet<ChangeFileEntity> ChangeFiles { get; set; }
        public DbSet<ChangeGroupEntity> ChangeGroups { get; set; }
        public DbSet<ChangeImplementationStatusEntity> ChangeImplementationStatuses { get; set; }
        public DbSet<ChangeLogEntity> ChangeLogs { get; set; }
        public DbSet<ChangeObjectEntity> ChangeObjects { get; set; }
        public DbSet<ChangePriorityEntity> ChangePriorities { get; set; }
        public DbSet<ChangeStatusEntity> ChangeStatuses { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistAction> ChecklistActions { get; set; }
        public DbSet<ChecklistRow> ChecklistRows { get; set; }
        public DbSet<ChecklistService> ChecklistServices { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<ComputerFieldSettings> ComputerFieldSettings { get; set; }
        public DbSet<ComputerHistory> ComputerHistories { get; set; }
        public DbSet<ComputerLog> ComputerLogs { get; set; }
        public DbSet<ComputerModel> ComputerModels { get; set; }
        public DbSet<ComputerType> ComputerTypes { get; set; }
        public DbSet<ComputerUser> ComputerUsers { get; set; }
        public DbSet<ComputerUserCustomerUserGroup> ComputerUserCustomerUserGroups { get; set; }
        public DbSet<ComputerUserFieldSettings> ComputerUserFieldSettings { get; set; }
        public DbSet<ComputerUserGroup> ComputerUserGroups { get; set; }
        public DbSet<ComputerUserLog> ComputerUserLogs { get; set; }
        public DbSet<ComputerUsersBlackList> ComputerUsersBlackLists { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractCategory> ContractCategories { get; set; }
        public DbSet<ContractFieldSettings> ContractFieldSettings { get; set; }
        public DbSet<ContractFile> ContractFiles { get; set; }
        public DbSet<ContractHistory> ContractHistories { get; set; }
        public DbSet<ContractLog> ContractLogs { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerUser> CustomerUsers { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }
        public DbSet<DailyReportSubject> DailyReportSubjects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentUser> DepartmentUsers { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Documentation> Documentations { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentCategory> DocumentCategories { get; set; }
        public DbSet<Helpdesk.Domain.Domain> Domains { get; set; }
        public DbSet<EmailGroupEntity> EMailGroups { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<FAQCategory> FAQCategories { get; set; }
        public DbSet<FAQFile> FAQFiles { get; set; }
        public DbSet<FAQLanguage> FAQLanguages { get; set; }
        public DbSet<FinishingCause> FinishingCauses { get; set; }
        public DbSet<FinishingCauseCategory> FinishingCauseCategories { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormField> FormField { get; set; }
        public DbSet<FormFieldValue> FormFieldValue { get; set; }
        public DbSet<GlobalSetting> GlobalSettings { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayHeader> HolidayHeaders { get; set; }
        public DbSet<Impact> Impacts { get; set; }
        public DbSet<InfoText> InfoTexts { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryType> InventoryTypes { get; set; }
        public DbSet<InventoryTypeProperty> InventoryTypeProperties { get; set; }
        public DbSet<InventoryTypePropertyValue> InventoryTypePropertyValues { get; set; }
        public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
        public DbSet<InvoiceRow> InvoiceRows { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<LicenseFile> LicenseFiles { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<LinkGroup> LinkGroups { get; set; }
        public DbSet<LocalAdmin> LocalAdmins { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogFile> LogFiles { get; set; }
        public DbSet<LogicalDrive> LogicalDrives { get; set; }
        public DbSet<LogProgram> LogPrograms { get; set; }
        public DbSet<LogSync> LogSyncs { get; set; }
        public DbSet<MailTemplate> MailTemplates { get; set; }
        public DbSet<MailTemplateLanguage> MailTemplateLanguages { get; set; }
        public DbSet<MailTemplateIdentifier> MailTemplateIdentifiers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<NIC> NICs { get; set; }
        public DbSet<OperatingSystem> OperatingSystems { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }
        public DbSet<OperationLogCategory> OperationLogCategories { get; set; }
        public DbSet<OperationLogEMailLog> OperationLogEMailLogs { get; set; }
        public DbSet<OperationObject> OperationObjects { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderEMailLog> OrderEMailLogs { get; set; }
        public DbSet<OrderFieldSettings> OrderFieldSettings { get; set; }
        public DbSet<OrderLog> OrderLogs { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<OU> OUs { get; set; }
        public DbSet<OULanguage> OULanguages { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionLanguage> PermissionLanguages { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<PrinterFieldSettings> PrinterFieldSettings { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<PriorityImpactUrgency> PriorityImpactUrgencies { get; set; }
        public DbSet<PriorityLanguage> PriorityLanguages { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemEMailLog> ProblemEMailLogs { get; set; }
        public DbSet<ProblemLog> ProblemLogs { get; set; }
        public DbSet<Processor> Processors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductArea> ProductAreas { get; set; }
        public DbSet<ProductAreaQuestion> ProductAreaQuestions { get; set; }
        public DbSet<ProductAreaQuestionVersion> ProductAreaQuestionVersions { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCollaborator> ProjectCollaborators { get; set; }
        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<ProjectLog> ProjectLogs { get; set; }
        public DbSet<ProjectSchedule> ProjectSchedules { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionGroup> QuestionGroups { get; set; }
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
            
        public DbSet<QuestionRegistration> QuestionRegistrations { get; set; }
        
        public DbSet<RAM> RAMs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionLanguage> RegionLanguages { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportCustomer> ReportCustomers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<ServerFieldSettings> ServerFieldSettings { get; set; }
        public DbSet<ServerLogicalDrive> ServerLogicalDrives { get; set; }
        public DbSet<ServerSoftware> ServerSoftwares { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<StandardText> StandardTexts { get; set; }
        public DbSet<StateSecondary> StateSecondaries { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Helpdesk.Domain.System> Systems { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<TextTranslation> TextTranslations { get; set; }
        public DbSet<TimeRegistration> TimeRegistrations { get; set; }
        public DbSet<TimeType> TimeTypes { get; set; }
        public DbSet<Urgency> Urgencies { get; set; }
        public DbSet<UrgencyLanguage> UrgencyLanguages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UsersPasswordHistory> UsersPasswordHistories { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserWorkingGroup> UserWorkingGroups { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<WatchDateCalendar> WatchDateCalendars { get; set; }
        public DbSet<WatchDateCalendarValue> WatchDateCalendarValues { get; set; }
        public DbSet<WorkingGroupEntity> WorkingGroups { get; set; }

        public DbSet<FAQCategoryLanguage> FaqCategoryLanguages { get; set; }

        public DbSet<ComputerUserFieldSettingsLanguage> ComputerUserFieldSettingsLanguages { get; set; }

        public DbSet<ChangeContactEntity> ChangeContacts { get; set; }

        public DbSet<ChangeDepartmentEntity> ChangeDepartments { get; set; }

        public DbSet<ChangeHistoryEntity> ChangeHistories { get; set; }

        public DbSet<ChangeChangeEntity> ChangeChanges { get; set; }

        public DbSet<ChangeChangeGroupEntity> ChangeChangeGroups { get; set; }
        
        public DbSet<QuestionnaireEntity> Questionnaires { get; set; }        

        public DbSet<QuestionnaireQuestionEntity> QuestionnaireQuestions { get; set; }

        public DbSet<QuestionnaireLanguageEntity> QuestionnaireLanguages { get; set; }

        public DbSet<QuestionnaireCircularPartEntity> QuestionnaireCircularParts { get; set; }

        public DbSet<QuestionnaireCircularEntity> QuestionnaireCirculars { get; set; }        
        
        public DbSet<QuestionnaireQuesLangEntity> QuestionnaireQuestionLanguage { get; set; }

        public DbSet<QuestionnaireQuesOpLangEntity> QuestionnaireQuestionOptionLanguage { get; set; }        

        public DbSet<QuestionnaireQuestionOptionEntity> QuestionnaireQuestionOptions { get; set; }

        public DbSet<QuestionnaireQuestionResultEntity> QuestionnaireQuestionResults { get; set; }

        public DbSet<QuestionnaireResultEntity> QuestionnaireResults { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // --------------------------------------------------------------------------------------------
            // ColumnTypeCasingConvention should be removed for dotConnect for Oracle.  
            // This option is obligatory only for SqlClient.  
            // --------------------------------------------------------------------------------------------

            #region modelBuilder.Configurations.Add(

            modelBuilder.Conventions.Remove<global::System.Data.Entity.Infrastructure.IncludeMetadataConvention>();

            modelBuilder.Configurations.Add(new AccountConfiguration());
            modelBuilder.Configurations.Add(new AccountActivityConfiguration());
            modelBuilder.Configurations.Add(new AccountActivityGroupConfiguration());
            modelBuilder.Configurations.Add(new ApplicationConfiguration());
            modelBuilder.Configurations.Add(new BuildingConfiguration());
            modelBuilder.Configurations.Add(new BulletinBoardConfiguration());
            modelBuilder.Configurations.Add(new CalendarConfiguration());
            modelBuilder.Configurations.Add(new CaseConfiguration());
            modelBuilder.Configurations.Add(new CaseSettingConfiguration());
            modelBuilder.Configurations.Add(new CaseFieldSettingConfiguration());
            modelBuilder.Configurations.Add(new CaseFieldSettingLanguageConfiguration());
            modelBuilder.Configurations.Add(new CaseFileConfiguration());
            modelBuilder.Configurations.Add(new CaseHistoryConfiguration());
            modelBuilder.Configurations.Add(new CaseInvoiceRowConfiguration());
            modelBuilder.Configurations.Add(new CaseQuestionCategoryConfiguration());
            modelBuilder.Configurations.Add(new CaseQuestionConfiguration());
            modelBuilder.Configurations.Add(new CaseQuestionHeaderConfiguration());
            modelBuilder.Configurations.Add(new CaseSolutionCategoryConfiguration());
            modelBuilder.Configurations.Add(new CaseSolutionConfiguration());
            modelBuilder.Configurations.Add(new CaseSolutionScheduleConfiguration());
            modelBuilder.Configurations.Add(new CaseTypeConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new ChangeConfiguration());
            modelBuilder.Configurations.Add(new ChangeCategoryConfiguration());
            modelBuilder.Configurations.Add(new ChangeGroupConfiguration());
            modelBuilder.Configurations.Add(new ChangeImplementationStatusConfiguration());
            modelBuilder.Configurations.Add(new ChangeObjectConfiguration());
            modelBuilder.Configurations.Add(new ChangePriorityConfiguration());
            modelBuilder.Configurations.Add(new ChangeStatusConfiguration());
            modelBuilder.Configurations.Add(new ChecklistActionConfiguration());
            modelBuilder.Configurations.Add(new ChecklistServiceConfiguration());
            modelBuilder.Configurations.Add(new ComputerUserConfiguration());
            modelBuilder.Configurations.Add(new ComputerUserFieldSettingsConfiguration());
            modelBuilder.Configurations.Add(new ComputerUserGroupConfiguration());
            modelBuilder.Configurations.Add(new ComputerUsersBlackListConfiguration());
            modelBuilder.Configurations.Add(new ComputerConfiguration());
            modelBuilder.Configurations.Add(new ComputerHistoryConfiguration());
            modelBuilder.Configurations.Add(new ComputerFieldSettingsConfiguration());
            modelBuilder.Configurations.Add(new ComputerLogConfiguration());
            modelBuilder.Configurations.Add(new ComputerModelConfiguration());
            modelBuilder.Configurations.Add(new ComputerTypeConfiguration());
            modelBuilder.Configurations.Add(new ContractCategoryConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new CurrencyConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new CustomerUserConfiguration());
            modelBuilder.Configurations.Add(new DailyReportSubjectConfiguration());
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new DepartmentUserConfiguration());
            modelBuilder.Configurations.Add(new DivisionConfiguration());
            modelBuilder.Configurations.Add(new DocumentCategoryConfiguration());
            modelBuilder.Configurations.Add(new DocumentConfiguration());
            modelBuilder.Configurations.Add(new DomainConfiguration());
            modelBuilder.Configurations.Add(new EMailGroupConfiguration());
            modelBuilder.Configurations.Add(new EmailLogConfiguration());
            modelBuilder.Configurations.Add(new FaqCategoryConfiguration());
            modelBuilder.Configurations.Add(new FaqConfiguration());
            modelBuilder.Configurations.Add(new FaqFileConfiguration());
            modelBuilder.Configurations.Add(new FaqLanguageConfiguration());
            modelBuilder.Configurations.Add(new FinishingCauseCategoryConfiguration());
            modelBuilder.Configurations.Add(new FinishingCauseConfiguration());
            modelBuilder.Configurations.Add(new FormConfiguration());
            modelBuilder.Configurations.Add(new FormFieldConfiguration());
            modelBuilder.Configurations.Add(new FormFieldValueConfiguration());
            modelBuilder.Configurations.Add(new FloorConfiguration());
            modelBuilder.Configurations.Add(new GlobalSettingConfiguration());
            modelBuilder.Configurations.Add(new HolidayConfiguration());
            modelBuilder.Configurations.Add(new HolidayHeaderConfiguration());
            modelBuilder.Configurations.Add(new ImpactConfiguration());
            modelBuilder.Configurations.Add(new InfoTextConfiguration());
            modelBuilder.Configurations.Add(new LanguageConfiguration());
            modelBuilder.Configurations.Add(new LinkConfiguration());
            modelBuilder.Configurations.Add(new LinkGroupConfiguration());
            modelBuilder.Configurations.Add(new LogConfiguration());
            modelBuilder.Configurations.Add(new LogFileConfiguration());
            modelBuilder.Configurations.Add(new MailTemplateConfiguration());
            modelBuilder.Configurations.Add(new MailTemplateIdentifierConfiguration());
            modelBuilder.Configurations.Add(new MailTemplateLanguageConfiguration());
            modelBuilder.Configurations.Add(new OperationLogConfiguration());
            modelBuilder.Configurations.Add(new OperationLogCategoryConfiguration());
            modelBuilder.Configurations.Add(new OperationObjectConfiguration());
            modelBuilder.Configurations.Add(new OperatingSystemConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new OrderStateConfiguration());
            modelBuilder.Configurations.Add(new OrderTypeConfiguration());
            modelBuilder.Configurations.Add(new OUConfiguration());
            modelBuilder.Configurations.Add(new PrinterConfiguration());
            modelBuilder.Configurations.Add(new PrinterFieldSettingsConfiguration());
            modelBuilder.Configurations.Add(new LogicalDriveConfiguration());
            modelBuilder.Configurations.Add(new NICConfiguration());
            modelBuilder.Configurations.Add(new ProcessorConfiguration());
            modelBuilder.Configurations.Add(new RAMConfiguration());
            modelBuilder.Configurations.Add(new SoftwareConfiguration());
            modelBuilder.Configurations.Add(new PriorityConfiguration());
            modelBuilder.Configurations.Add(new PriorityLanguageConfiguration());
            modelBuilder.Configurations.Add(new PriorityImpactUrgencyConfiguration());
            modelBuilder.Configurations.Add(new ProblemConfiguration());
            modelBuilder.Configurations.Add(new ProblemLogConfiguration());
            modelBuilder.Configurations.Add(new ProblemEmailLogConfiguration());
            modelBuilder.Configurations.Add(new ProductAreaConfiguration());
            modelBuilder.Configurations.Add(new ProgramConfiguration());
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Configurations.Add(new ProjectLogConfiguration());
            modelBuilder.Configurations.Add(new ProjectScheduleConfiguration());
            modelBuilder.Configurations.Add(new ProjectCollaboratorConfiguration());
            modelBuilder.Configurations.Add(new ProjectFileConfiguration());
            modelBuilder.Configurations.Add(new QuestionGroupConfiguration());
            modelBuilder.Configurations.Add(new RegionConfiguration());
            modelBuilder.Configurations.Add(new ReportConfiguration());
            modelBuilder.Configurations.Add(new ReportCustomerConfiguration());
            modelBuilder.Configurations.Add(new RoomConfiguration());
            modelBuilder.Configurations.Add(new ServerConfiguration());
            modelBuilder.Configurations.Add(new ServerFieldSettingsConfiguration());
            modelBuilder.Configurations.Add(new ServerLogicalDriveConfiguration());
            modelBuilder.Configurations.Add(new ServerSoftwareConfiguration());
            modelBuilder.Configurations.Add(new SettingConfiguration());
            modelBuilder.Configurations.Add(new StandardTextConfiguration());
            modelBuilder.Configurations.Add(new StateSecondaryConfiguration());
            modelBuilder.Configurations.Add(new StatusConfiguration());
            modelBuilder.Configurations.Add(new SupplierConfiguration());
            modelBuilder.Configurations.Add(new SystemConfiguration());
            modelBuilder.Configurations.Add(new TextConfiguration());
            modelBuilder.Configurations.Add(new TimeTypeConfiguration());
            modelBuilder.Configurations.Add(new TextTranslationConfiguration());
            modelBuilder.Configurations.Add(new UrgencyConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserGroupConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new UserWorkingGroupConfiguration());
            modelBuilder.Configurations.Add(new WatchDateCalendarConfiguration());
            modelBuilder.Configurations.Add(new WatchDateCalendarValueConfiguration());
            modelBuilder.Configurations.Add(new WorkingGroupConfiguration());
            modelBuilder.Configurations.Add(new FaqCategoryLanguageConfiguration());
            //modelBuilder.Configurations.Add(new ComputerUserFieldSettingsLanguageConfiguration());
            modelBuilder.Configurations.Add(new ChangeFieldSettingsConfiguration());
            modelBuilder.Configurations.Add(new ChangeContactConfiguration());
            modelBuilder.Configurations.Add(new ChangeDepartmentConfiguration());
            modelBuilder.Configurations.Add(new ChangeHistoryConfiguration());
            modelBuilder.Configurations.Add(new ChangeEmailLogConfiguration());
            modelBuilder.Configurations.Add(new ChangeLogConfiguration());
            modelBuilder.Configurations.Add(new ChangeFileConfiguration());
            modelBuilder.Configurations.Add(new ChangeChangeConfiguration());
            modelBuilder.Configurations.Add(new ChangeChangeGroupConfiguration());
   
            modelBuilder.Configurations.Add(new QuestionnaireConfiguration());
            modelBuilder.Configurations.Add(new QuestionnaireQuestionConfiquration());
            modelBuilder.Configurations.Add(new QuestionnaireLanguageConfiguration());
            modelBuilder.Configurations.Add(new QuestionnaireCircularConfiguration());
            modelBuilder.Configurations.Add(new QuestionnaireCircularPartConfiguration());            
            modelBuilder.Configurations.Add(new QuestionnaireQuesOpLangConfiguration());
            modelBuilder.Configurations.Add(new QuestionnaireQuestionOptionConfiguration());
            modelBuilder.Configurations.Add(new QuestionnaireResultConfiguration());
            modelBuilder.Configurations.Add(new QuestionnaireQuesLangConfiguration());
            modelBuilder.Configurations.Add(new QuestionnaireQuestionResultConfiguration());

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public virtual void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch (global::System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                StringBuilder sb = new StringBuilder(); 
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.Append(validationError.PropertyName + " " + validationError.ErrorMessage);
                        global::System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new global::System.Exception(sb.ToString()); 
            }
        }
    }
}
