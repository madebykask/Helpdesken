using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class SettingConfiguration : EntityTypeConfiguration<Setting>
    {
        internal SettingConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.CloseOrderState)
                .WithMany(x => x.Settings)
                .HasForeignKey(x => x.CloseOrder_OrderState_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.ADSyncURL).IsRequired().HasMaxLength(100);
            Property(x => x.CaseArchiveDays).IsRequired();
            Property(x => x.CaseComplaintDays).IsRequired();
            Property(x => x.CaseDateFormat).IsRequired();
            Property(x => x.CaseOverviewInfo).IsOptional().HasMaxLength(1000);
            Property(x => x.CaseSMS).IsRequired();
            Property(x => x.CaseWorkingGroupSource).IsRequired();
            Property(x => x.CategoryFilterFormat).IsRequired();
            Property(x => x.CloseCase_finishingCause_Id).IsOptional();
            Property(x => x.CaseFiles).IsOptional();
            //Property(x => x.CloseOrder_OrderState_Id).IsOptional();
            Property(x => x.ComplexPassword).IsRequired();
            Property(x => x.ComputerDepartmentSource).IsRequired();
            Property(x => x.ComputerLog).IsRequired();
            Property(x => x.ComputerUserInfoListLocation).IsRequired();
            Property(x => x.ComputerUserLog).IsRequired();
            Property(x => x.CreateCaseFromOrder).IsRequired();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.DBType).IsRequired();
            Property(x => x.DefaultAdministrator).IsOptional();
            Property(x => x.DefaultAdministratorExternal).IsOptional();
            Property(x => x.DepartmentFilterFormat).IsRequired();
            Property(x => x.DepartmentFormat).IsRequired();
            Property(x => x.DisableCaseEndDate).IsRequired();
            Property(x => x.DontConnectUserToWorkingGroup).IsRequired();
            Property(x => x.DSN_Sync).IsRequired().HasMaxLength(200);
            Property(x => x.EMailAnswerDestination).IsRequired();
            Property(x => x.EMailAnswerSeparator).IsRequired().HasMaxLength(20);
            Property(x => x.EMailImportType).IsRequired();
            Property(x => x.EMailRegistrationMailID).IsRequired();
            Property(x => x.EMailSubjectPattern).IsRequired().HasMaxLength(20);
            Property(x => x.InventoryDays2WaitBeforeDelete).IsOptional();
            Property(x => x.InvoiceType).IsRequired();
            Property(x => x.LDAPAllUsers).IsRequired();
            Property(x => x.LDAPAuthenticationType).IsRequired();
            Property(x => x.LDAPBase).IsRequired().HasMaxLength(20);
            Property(x => x.LDAPFilter).IsRequired().HasMaxLength(50);
            Property(x => x.LDAPLogLevel).IsRequired();
            Property(x => x.LDAPPassword).IsRequired().HasMaxLength(20);
            Property(x => x.LDAPSyncType).IsRequired();
            Property(x => x.LDAPUserName).IsRequired().HasMaxLength(100);
            Property(x => x.LeadTimeFromProductAreaSetDate).IsRequired();
            Property(x => x.LogLevel).IsRequired();
            Property(x => x.LogNoteFormat).IsRequired();
            Property(x => x.MailServerProtocol).IsRequired();
            Property(x => x.MarkCaseUnread).IsRequired();
            Property(x => x.MaxPasswordAge).IsRequired();
            Property(x => x.MinPasswordLength).IsRequired();
            Property(x => x.ModuleADSync).IsRequired();
            Property(x => x.ModuleAsset).IsRequired();
            Property(x => x.ModuleBulletinBoard).IsRequired();
            Property(x => x.ModuleCalendar).IsRequired();
            Property(x => x.ModuleCase).IsRequired();
            Property(x => x.ModuleChangeManagement).IsRequired();
            Property(x => x.ModuleChecklist).IsRequired();
            Property(x => x.ModuleComputerUser).IsRequired();
            Property(x => x.ModuleContract).IsRequired();
            Property(x => x.ModuleDailyReport).IsRequired();
            Property(x => x.ModuleDocument).IsRequired();
            Property(x => x.ModuleFAQ).IsRequired();
            Property(x => x.ModuleInventory).IsRequired();
            Property(x => x.ModuleInventoryImport).IsRequired();
            Property(x => x.ModuleInvoice).IsRequired();
            Property(x => x.ModuleLicense).IsRequired();
            Property(x => x.ModuleOperationLog).IsRequired();
            Property(x => x.ModuleOrder).IsRequired();
            Property(x => x.ModulePlanning).IsRequired();
            Property(x => x.ModuleProblem).IsRequired();
            Property(x => x.ModuleProject).IsRequired();
            Property(x => x.ModuleQuestion).IsRequired();
            Property(x => x.ModuleQuestionnaire).IsRequired();
            Property(x => x.ModuleTimeRegistration).IsRequired();
            Property(x => x.ModuleWatch).IsRequired();
            Property(x => x.NoMailToNotifierChecked).IsRequired();
            Property(x => x.PasswordHistory).IsRequired();
            Property(x => x.PlanDateFormat).IsRequired();
            Property(x => x.POP3DebugLevel).IsRequired();
            Property(x => x.POP3EMailPrefix).IsRequired().HasMaxLength(20);
            Property(x => x.POP3Password).IsRequired().HasMaxLength(20);
            Property(x => x.POP3Server).IsRequired().HasMaxLength(50);
            Property(x => x.POP3UserName).IsRequired().HasMaxLength(20);
            Property(x => x.PriorityFormat).IsRequired();
            Property(x => x.ProductAreaFilterFormat).IsRequired();
            Property(x => x.ProductAreaFormat).IsRequired();
            Property(x => x.SearchCaseOnExternalPage).IsRequired();
            Property(x => x.SelfServiceEmailReminder).IsRequired();
            Property(x => x.SetFirstUserToOwner).IsRequired();
            Property(x => x.ShowCaseOverviewInfo).IsRequired();
            Property(x => x.ShowStatusPanel).IsRequired();
            Property(x => x.SMSEMailDomain).IsRequired().HasMaxLength(50);
            Property(x => x.SMSEMailDomainPassword).IsRequired().HasMaxLength(20);
            Property(x => x.SMSEMailDomainUserId).IsRequired().HasMaxLength(20);
            Property(x => x.SMSEMailDomainUserName).IsRequired().HasMaxLength(20);
            Property(x => x.StateSecondaryFilterFormat).IsRequired();
            Property(x => x.StateSecondaryFormat).IsRequired();
            Property(x => x.StateSecondaryReminder).IsRequired();
            Property(x => x.XMLAllFiles).IsRequired();
            Property(x => x.XMLFileFolder).IsOptional().HasMaxLength(100);
            Property(x => x.XMLLogLevel).IsRequired();
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblsettings");
        }
    }
}
