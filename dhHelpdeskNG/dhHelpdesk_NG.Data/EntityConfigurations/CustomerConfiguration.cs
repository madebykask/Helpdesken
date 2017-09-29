namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        internal CustomerConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasMany(u => u.Users)
                .WithMany(a => a.Cs)
                .Map(m =>
                {
                    m.MapLeftKey("Customer_Id");
                    m.MapRightKey("User_Id");
                    m.ToTable("tblCustomerUser");
                });

            this.HasMany(u => u.UsersAvailable)
                .WithMany(c => c.CusomersAvailable)
                .Map(m =>
                {
                    m.MapLeftKey("Customer_Id");
                    m.MapRightKey("User_Id");
                    m.ToTable("tblCustomerAvailableUser");
                });

            this.HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(o => o.ReportCustomers)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(o => o.ReportFavorites)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Address).IsRequired().HasMaxLength(50);
            this.Property(x => x.CaseStatisticsEmailList).IsRequired().HasMaxLength(500);
            this.Property(x => x.ControlTime).IsRequired();
            this.Property(x => x.CustomerGroup_Id).IsOptional();
            this.Property(x => x.CustomerGUID).IsRequired();
            this.Property(x => x.CustomerID).IsRequired().HasMaxLength(20);
            this.Property(x => x.CustomerNumber).IsRequired().HasMaxLength(20);
            this.Property(x => x.DailyReportEmail).IsRequired().HasMaxLength(500);
            this.Property(x => x.Days2WaitBeforeDelete).IsRequired();
            this.Property(x => x.DirectoryPathExclude).IsRequired().HasMaxLength(1500);
            this.Property(x => x.EMailSendFromOrder).IsRequired();
            this.Property(x => x.HelpdeskEmail).IsRequired().HasMaxLength(100);
            this.Property(x => x.Language_Id).IsRequired();
            this.Property(x => x.LockCaseToWorkingGroup).IsRequired();
            this.Property(x => x.Logo).IsRequired().HasMaxLength(50);
            this.Property(x => x.LogoBackColor).IsRequired().HasMaxLength(10);
            this.Property(x => x.MarkPassedWatchDateAcute).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50);
            this.Property(x => x.NDSPath).IsRequired().HasMaxLength(200);
            this.Property(x => x.NewCaseEmailList).IsRequired().HasMaxLength(500);
            this.Property(x => x.OrderEMailList).IsRequired().HasMaxLength(500);
            this.Property(x => x.OrderPermission).IsRequired();
            this.Property(x => x.OverwriteFromMasterDirectory).IsRequired();
            this.Property(x => x.PasswordRequiredOnExternalPage).IsRequired();
            this.Property(x => x.Phone).IsRequired().HasMaxLength(20);
            this.Property(x => x.PostalAddress).IsRequired().HasMaxLength(30);
            this.Property(x => x.PostalCode).IsRequired().HasMaxLength(8);
            this.Property(x => x.RegistrationMessage).IsRequired().HasMaxLength(1000);
            this.Property(x => x.ResponsibleReminderControlTime).IsRequired();
            this.Property(x => x.ResponsibleReminderEmailList).IsRequired().HasMaxLength(400);
            this.Property(x => x.ShowBulletinBoardOnExtPage).IsRequired();
            this.Property(x => x.ShowCaseOnExternalPage).IsRequired();
            this.Property(x => x.ShowDashboardOnExternalPage).IsRequired();
            this.Property(x => x.ShowFAQOnExternalPage).IsRequired();
            this.Property(x => x.WorkingDayEnd).IsRequired();
            this.Property(x => x.WorkingDayStart).IsRequired();
            this.Property(x => x.ChangeTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.RegTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.CommunicateWithNotifier).IsRequired();
            this.Property(x => x.ShowDocumentsOnExternalPage).IsRequired();
            this.Property(x => x.ShowFAQOnExternalStartPage).IsOptional();
            this.Property(x => x.ShowCoWorkersOnExternalPage).IsRequired();
            this.Property(x => x.ShowHelpOnExternalPage).IsRequired();
            this.Property(x => x.UseInternalLogNoteOnExternalPage).IsRequired();
            this.Property(x => x.MyCasesFollower).IsRequired();
            this.Property(x => x.MyCasesInitiator).IsRequired();
            this.Property(x => x.MyCasesRegarding).IsRequired();
            this.Property(x => x.MyCasesRegistrator).IsRequired();
            this.Property(x => x.MyCasesUserGroup).IsRequired();
            this.Property(x => x.RestrictUserToGroupOnExternalPage).IsRequired();
            this.Property(x => x.FetchDataFromApiOnExternalPage).IsRequired();
            this.Property(x => x.ShowCasesOnExternalPage).IsRequired();
            this.Property(x => x.GroupCaseTemplates).IsRequired();

            this.ToTable("tblcustomer");
        }
    }
}
