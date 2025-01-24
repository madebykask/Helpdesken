
namespace DH.Helpdesk.Domain
{
    public class Setting : Entity
    {
        public int CategoryFilterFormat { get; set; }
        public int CaseArchiveDays { get; set; }
        public int CaseComplaintDays { get; set; }
        public int CaseDateFormat { get; set; }
        public int CaseSMS { get; set; }
        public int CaseFiles { get; set; }  //READ ONLY, DON'T CHANGE IT FROM PROJECT- ONLY IN DB IF NECESSARY!

        /// <summary>
        /// Mode of displaying workging group [(http://localhost/admin/customersettings/edit/4) "Case Overview" secion]
        /// 0 - only display value if case is assigned to user’s default working group (field where other working group will be displayed as empty)
        /// 1 - always display value
        /// </summary>
        public int CaseWorkingGroupSource { get; set; }

        public int? CloseCase_finishingCause_Id { get; set; }
        public int? CloseOrder_OrderState_Id { get; set; }
        public int ComplexPassword { get; set; }
        public int ComputerDepartmentSource { get; set; }
        public int ComputerLog { get; set; }
        public int CustomerInExtendedSearch { get; set; }
        public int ComputerUserSearchRestriction { get; set; }
        public int ComputerUserInfoListLocation { get; set; }  //READ ONLY, DON'T CHANGE IT FROM PROJECT- ONLY IN DB IF NECESSARY!
        public int ComputerUserLog { get; set; }
        public int CreateCaseFromOrder { get; set; }
        public int CreateComputerFromOrder { get; set; }
        public int Customer_Id { get; set; }
        public int DBType { get; set; }
        public int? DefaultAdministrator { get; set; }
        public int? DefaultAdministratorExternal { get; set; }
        public int DepartmentFilterFormat { get; set; }
        public int DepartmentFormat { get; set; }
        public int DisableCaseEndDate { get; set; }
        public int DontConnectUserToWorkingGroup { get; set; }
        public int EMailAnswerDestination { get; set; }
        public int EMailImportType { get; set; }
        public int EMailRegistrationMailID { get; set; }
        //public int? InventoryDays2WaitBeforeDelete { get; set; }
        public int InvoiceType { get; set; }
        public int LDAPAllUsers { get; set; }
        public int LDAPAuthenticationType { get; set; }
        public int LDAPLogLevel { get; set; }
        public int LDAPSyncType { get; set; }
        public int LDAPCreateOrganization { get; set; }
        public int LeadTimeFromProductAreaSetDate { get; set; }
        public int LogLevel { get; set; }
        public int LogNoteFormat { get; set; }
        public int MailServerProtocol { get; set; }
        //public int MarkCaseUnread { get; set; }
        public int MaxPasswordAge { get; set; }
        public int MinPasswordLength { get; set; }
        public int MinRegWorkingTime { get; set; }
        public int ModuleAccount { get; set; }
        public int ModuleADSync { get; set; }
        public int ModuleAsset { get; set; }
        public int ModuleBulletinBoard { get; set; }
        public int ModuleCalendar { get; set; }
        public int ModuleCase { get; set; }  //READ ONLY, DON'T CHANGE IT FROM PROJECT- ONLY IN DB IF NECESSARY!
        public int ModuleChangeManagement { get; set; }
        public int ModuleChecklist { get; set; }
        public int ModuleComputerUser { get; set; }
        public int ModuleContract { get; set; }
        public int ModuleDailyReport { get; set; }
        public int ModuleDocument { get; set; }
        public int ModuleFAQ { get; set; }
        public int ModuleInventory { get; set; }
        public int ModuleInventoryImport { get; set; }
        public int ModuleInvoice { get; set; }
        public int ModuleLicense { get; set; }
        public int ModuleOperationLog { get; set; }
        public int ModuleOrder { get; set; }
        public int ModulePlanning { get; set; }
        public int ModuleProject { get; set; }
        public int ModuleProblem { get; set; }
        public int ModuleQuestion { get; set; }
        public int ModuleQuestionnaire { get; set; }
        public int ModuleTimeRegistration { get; set; }
        public int ModuleWatch { get; set; }
        public int NoMailToNotifierChecked { get; set; }
        public int PasswordHistory { get; set; }
        public int PlanDateFormat { get; set; }
        public int POP3DebugLevel { get; set; }
        public int POP3Port { get; set; }
        public int PriorityFormat { get; set; }
        public int ProductAreaFilterFormat { get; set; }
        public int ProductAreaFormat { get; set; }
        public int SearchCaseOnExternalPage { get; set; }
        public int SelfServiceEmailReminder { get; set; }
        public int SetFirstUserToOwner { get; set; }
        public int SetUserToAdministrator { get; set; }
        public int ShowCaseOverviewInfo { get; set; }
        public int ShowStatusPanel { get; set; }
        public int StateSecondaryFilterFormat { get; set; }
        public int StateSecondaryFormat { get; set; }
        public int StateSecondaryReminder { get; set; }
        public int XMLAllFiles { get; set; }
        public int XMLLogLevel { get; set; }
        public string ADSyncURL { get; set; }
        public string CaseOverviewInfo { get; set; }
        public string DSN_Sync { get; set; }
        public string EMailAnswerSeparator { get; set; }
        public string EMailSubjectPattern { get; set; }
        public string ExternalEMailSubjectPattern { get; set; }
        public int IntegrationType { get; set; }
        public string LDAPBase { get; set; }
        public string LDAPFilter { get; set; }
        public string LDAPPassword { get; set; }
        public string LDAPUserName { get; set; }
        public string POP3EMailPrefix { get; set; }
        public string POP3Password { get; set; }
        public string POP3Server { get; set; }
        public string POP3UserName { get; set; }
        public string SMSEMailDomain { get; set; }
        public string SMSEMailDomainPassword { get; set; }
        public string SMSEMailDomainUserId { get; set; }
        public string SMSEMailDomainUserName { get; set; }
        public string XMLFileFolder { get; set; }
        public string PhysicalFilePath { get; set; }
        public string VirtualFilePath { get; set; }               
        public int ModuleCaseInvoice { get; set; }
        public int PreventToSaveCaseWithInactiveValue { get; set; }
        public int ShowOUsOnDepartmentFilter { get; set; }
        public string FileIndexingServerName { get; set; }
        public string FileIndexingCatalogName { get; set; }
        public int DefaultEmailLogDestination { get; set; }
        public int CalcSolvedInTimeByLatestSLADate { get; set; }                   
        public int TimeZone_offset { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassWord { get; set; }
        public bool IsSMTPSecured { get; set; }
        public bool BatchEmail { get; set; }
        public int BulletinBoardWGRestriction { get; set; }
        public int CalendarWGRestriction { get; set; }
        public int ModuleExtendedCase { get; set; }
        public int AttachmentPlacement { get; set; }
        public int M2TNewCaseMailTo { get; set; }
        public virtual OrderState CloseOrderState { get; set; }
        /// <summary>
        /// If 1 - indicates that users should be presented(and sorted by) as "FirstName LastName", and vice versa
        /// </summary>
        public int IsUserFirstLastNameRepresentation { get; set; }

        public int ShowQuickNewCaseLink { get; set; }
        public string QuickNewCaseLinkText { get; set; }
        public string QuickNewCaseLinkUrl { get; set; }
        public int DefaultCaseTemplateId { get; set; }
        public bool QuickLinkWGRestriction { get; set; }
        public bool AllowMoveCaseToAnyCustomer { get; set; }
        public string BlockedEmailRecipients { get; set; }

        public string EMailFolder { get; set; }

        public string EMailFolderArchive { get; set; }

        public bool UseEws { get; set; }

        public string EwsApplicationId { get; set; }

        public string EwsClientSecret { get; set; }

        public string EwsTenantId { get; set; }
        public bool UseGraphSendingEmail { get; set; }
        public string GraphClientId { get; set; }
        public string GraphUserName { get; set; }
        public string GraphTenantId { get; set; }
        public string GraphClientSecret { get; set; }
        public string SharePointSiteId { get; set; }
        public string SharePointUserName { get; set; }
        public string SharePointPassword { get; set; }
        public string SharePointFolderId { get; set; }
        public string SharePointDriveId { get; set; }

        public string SharePointSecretKey { get; set; }
        public string SharePointClientId { get; set; }
        public string SharePointTenantId { get; set; }
        public string SharePointScope { get; set; }

        public string ErrorMailTo { get; set; }
    }
}
