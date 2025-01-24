namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class SettingConfiguration : EntityTypeConfiguration<Setting>
    {
        internal SettingConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.CloseOrderState)
                .WithMany(x => x.Settings)
                .HasForeignKey(x => x.CloseOrder_OrderState_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ADSyncURL).IsRequired().HasMaxLength(100);
            this.Property(x => x.CaseArchiveDays).IsRequired();
            this.Property(x => x.CaseComplaintDays).IsRequired();
            this.Property(x => x.CaseDateFormat).IsRequired();
            this.Property(x => x.CaseOverviewInfo).IsOptional().HasMaxLength(1000);
            this.Property(x => x.CaseSMS).IsRequired();
            this.Property(x => x.CaseWorkingGroupSource).IsRequired();
            this.Property(x => x.CategoryFilterFormat).IsRequired();
            this.Property(x => x.CloseCase_finishingCause_Id).IsOptional();
            this.Property(x => x.CaseFiles).IsOptional();
            //Property(x => x.CloseOrder_OrderState_Id).IsOptional();
            this.Property(x => x.ComplexPassword).IsRequired();
            this.Property(x => x.ComputerDepartmentSource).IsRequired();
            this.Property(x => x.ComputerLog).IsRequired();
            this.Property(x => x.ComputerUserInfoListLocation).IsRequired();
            this.Property(x => x.CustomerInExtendedSearch).IsRequired();
            this.Property(x => x.ComputerUserLog).IsRequired();
            this.Property(x => x.CreateCaseFromOrder).IsRequired();
            this.Property(x => x.CreateComputerFromOrder).IsRequired();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.DBType).IsRequired();
            this.Property(x => x.DefaultAdministrator).IsOptional();
            this.Property(x => x.DefaultAdministratorExternal).IsOptional();
            this.Property(x => x.DepartmentFilterFormat).IsRequired();
            this.Property(x => x.DepartmentFormat).IsRequired();
            this.Property(x => x.DisableCaseEndDate).IsRequired();
            this.Property(x => x.DontConnectUserToWorkingGroup).IsRequired();
            this.Property(x => x.DSN_Sync).IsRequired().HasMaxLength(200);
            this.Property(x => x.EMailAnswerDestination).IsRequired();
            this.Property(x => x.EMailAnswerSeparator).IsRequired().HasMaxLength(512);
            this.Property(x => x.EMailImportType).IsRequired();
            this.Property(x => x.EMailRegistrationMailID).IsRequired();            
            this.Property(x => x.EMailSubjectPattern).IsRequired().HasMaxLength(100);
            this.Property(x => x.ExternalEMailSubjectPattern).IsRequired().HasMaxLength(1000);
            //this.Property(x => x.InventoryDays2WaitBeforeDelete).IsOptional();
            this.Property(x => x.InvoiceType).IsRequired();
            this.Property(x => x.IntegrationType).IsRequired();
            this.Property(x => x.LDAPAllUsers).IsRequired();
            this.Property(x => x.LDAPAuthenticationType).IsRequired();
            this.Property(x => x.LDAPBase).IsRequired().HasMaxLength(200);
            this.Property(x => x.LDAPFilter).IsRequired().HasMaxLength(150);
            this.Property(x => x.LDAPLogLevel).IsRequired();
            this.Property(x => x.LDAPPassword).IsRequired().HasMaxLength(20);
            this.Property(x => x.LDAPSyncType).IsRequired();
            this.Property(x => x.LDAPCreateOrganization).IsRequired();
            this.Property(x => x.LDAPUserName).IsRequired().HasMaxLength(100);
            this.Property(x => x.LeadTimeFromProductAreaSetDate).IsRequired();
            this.Property(x => x.LogLevel).IsRequired();
            this.Property(x => x.LogNoteFormat).IsRequired();
            this.Property(x => x.MailServerProtocol).IsRequired();
            //this.Property(x => x.MarkCaseUnread).IsRequired();
            this.Property(x => x.MaxPasswordAge).IsRequired();
            this.Property(x => x.MinPasswordLength).IsRequired();
            this.Property(x => x.MinRegWorkingTime).IsRequired();
            this.Property(x => x.ModuleAccount).IsRequired();
            this.Property(x => x.ModuleADSync).IsRequired();
            this.Property(x => x.ModuleAsset).IsRequired();
            this.Property(x => x.ModuleBulletinBoard).IsRequired();
            this.Property(x => x.ModuleCalendar).IsRequired();
            this.Property(x => x.ModuleCase).IsRequired();
            this.Property(x => x.ModuleChangeManagement).IsRequired();
            this.Property(x => x.ModuleChecklist).IsRequired();
            this.Property(x => x.ModuleComputerUser).IsRequired();
            this.Property(x => x.ModuleContract).IsRequired();
            this.Property(x => x.ModuleDailyReport).IsRequired();
            this.Property(x => x.ModuleDocument).IsRequired();
            this.Property(x => x.ModuleFAQ).IsRequired();
            this.Property(x => x.ModuleInventory).IsRequired();
            this.Property(x => x.ModuleInventoryImport).IsRequired();
            this.Property(x => x.ModuleInvoice).IsRequired();
            this.Property(x => x.ModuleLicense).IsRequired();
            this.Property(x => x.ModuleOperationLog).IsRequired();
            this.Property(x => x.ModuleOrder).IsRequired();
            this.Property(x => x.ModulePlanning).IsRequired();
            this.Property(x => x.ModuleProblem).IsRequired();
            this.Property(x => x.ModuleProject).IsRequired();
            this.Property(x => x.ModuleQuestion).IsRequired();
            this.Property(x => x.ModuleQuestionnaire).IsRequired();
            this.Property(x => x.ModuleTimeRegistration).IsRequired();
            this.Property(x => x.ModuleWatch).IsRequired();
            this.Property(x => x.NoMailToNotifierChecked).IsRequired();
            this.Property(x => x.PasswordHistory).IsRequired();
            this.Property(x => x.PlanDateFormat).IsRequired();
            this.Property(x => x.POP3DebugLevel).IsRequired();
            this.Property(x => x.POP3Port).IsRequired();
            this.Property(x => x.POP3EMailPrefix).IsRequired().HasMaxLength(20);
            this.Property(x => x.POP3Password).IsRequired().HasMaxLength(40);
            this.Property(x => x.POP3Server).IsRequired().HasMaxLength(50);
            this.Property(x => x.POP3UserName).IsRequired().HasMaxLength(50);
            this.Property(x => x.PriorityFormat).IsRequired();
            this.Property(x => x.ProductAreaFilterFormat).IsRequired();
            this.Property(x => x.ProductAreaFormat).IsRequired();
            this.Property(x => x.SearchCaseOnExternalPage).IsRequired();
            this.Property(x => x.SelfServiceEmailReminder).IsRequired();
            this.Property(x => x.SetFirstUserToOwner).IsRequired();
            this.Property(x => x.ShowCaseOverviewInfo).IsRequired();
            this.Property(x => x.ShowStatusPanel).IsRequired();
            this.Property(x => x.SMSEMailDomain).IsRequired().HasMaxLength(50);
            this.Property(x => x.SMSEMailDomainPassword).IsRequired().HasMaxLength(20);
            this.Property(x => x.SMSEMailDomainUserId).IsRequired().HasMaxLength(20);
            this.Property(x => x.SMSEMailDomainUserName).IsRequired().HasMaxLength(20);
            this.Property(x => x.StateSecondaryFilterFormat).IsRequired();
            this.Property(x => x.StateSecondaryFormat).IsRequired();
            this.Property(x => x.StateSecondaryReminder).IsRequired();
            this.Property(x => x.XMLAllFiles).IsRequired();
            this.Property(x => x.XMLFileFolder).IsOptional().HasMaxLength(100);
            this.Property(x => x.XMLLogLevel).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.ModuleCaseInvoice).IsRequired();
            this.Property(x => x.PhysicalFilePath).IsOptional();
            this.Property(x => x.VirtualFilePath).IsOptional();
            this.Property(x => x.IsUserFirstLastNameRepresentation).IsRequired();
            this.Property(x => x.PreventToSaveCaseWithInactiveValue).IsRequired();
            this.Property(x => x.ShowOUsOnDepartmentFilter).IsRequired();
            this.Property(x => x.FileIndexingServerName).IsOptional().HasMaxLength(200);
            this.Property(x => x.FileIndexingCatalogName).IsOptional().HasMaxLength(200);
            this.Property(x => x.DefaultEmailLogDestination).IsRequired();
            this.Property(x => x.CalcSolvedInTimeByLatestSLADate).IsRequired();
            this.Property(x => x.TimeZone_offset).IsRequired();
            this.Property(x => x.SetUserToAdministrator).IsRequired();
            this.Property(x => x.SMTPServer).IsOptional();
            this.Property(x => x.SMTPPort).IsRequired();
            this.Property(x => x.SMTPUserName).IsOptional();
            this.Property(x => x.SMTPPassWord).IsOptional();
            this.Property(x => x.UseGraphSendingEmail).IsRequired();
            this.Property(x => x.GraphClientId).IsOptional();
            this.Property(x => x.GraphUserName).IsOptional();
            this.Property(x => x.GraphClientSecret).IsOptional();
            this.Property(x => x.GraphTenantId).IsOptional();
            this.Property(x => x.IsSMTPSecured).IsRequired();
            this.Property(x => x.BulletinBoardWGRestriction).IsRequired();
            this.Property(x => x.CalendarWGRestriction).IsRequired();
            this.Property(x => x.ModuleExtendedCase).IsRequired();
            this.Property(x => x.AttachmentPlacement).IsRequired();
            this.Property(x => x.M2TNewCaseMailTo).IsRequired();
            this.Property(x => x.DefaultCaseTemplateId).IsRequired();
            this.Property(x => x.QuickLinkWGRestriction).IsRequired();
            this.Property(x => x.AllowMoveCaseToAnyCustomer).IsRequired();
			this.ToTable("tblsettings");
        }
    }
}
