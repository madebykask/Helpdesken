using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        internal CustomerConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(u => u.Users)
                .WithMany(a => a.Cs)
                .Map(m =>
                {
                    m.MapLeftKey("Customer_Id");
                    m.MapRightKey("User_Id");
                    m.ToTable("tblCustomerUser");
                });

            HasRequired(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.Language_Id)
                .WillCascadeOnDelete(false);

            HasMany(o => o.ReportCustomers)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.Customer_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Address).IsRequired().HasMaxLength(50);
            Property(x => x.CaseStatisticsEmailList).IsRequired().HasMaxLength(500);
            Property(x => x.ControlTime).IsRequired();
            Property(x => x.CustomerGroup_Id).IsOptional();
            Property(x => x.CustomerGUID).IsRequired();
            Property(x => x.CustomerID).IsRequired().HasMaxLength(20);
            Property(x => x.CustomerNumber).IsRequired().HasMaxLength(20);
            Property(x => x.DailyReportEmail).IsRequired().HasMaxLength(500);
            Property(x => x.Days2WaitBeforeDelete).IsRequired();
            Property(x => x.DirectoryPathExclude).IsRequired().HasMaxLength(1500);
            Property(x => x.EMailSendFromOrder).IsRequired();
            Property(x => x.HelpdeskEmail).IsRequired().HasMaxLength(100);
            Property(x => x.Language_Id).IsRequired();
            Property(x => x.LockCaseToWorkingGroup).IsRequired();
            Property(x => x.Logo).IsRequired().HasMaxLength(50);
            Property(x => x.LogoBackColor).IsRequired().HasMaxLength(10);
            Property(x => x.MarkPassedWatchDateAcute).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50);
            Property(x => x.NDSPath).IsRequired().HasMaxLength(200);
            Property(x => x.NewCaseEmailList).IsRequired().HasMaxLength(500);
            Property(x => x.OrderEMailList).IsRequired().HasMaxLength(500);
            Property(x => x.OrderPermission).IsRequired();
            Property(x => x.OverwriteFromMasterDirectory).IsRequired();
            Property(x => x.PasswordRequiredOnExternalPage).IsRequired();
            Property(x => x.Phone).IsRequired().HasMaxLength(20);
            Property(x => x.PostalAddress).IsRequired().HasMaxLength(30);
            Property(x => x.PostalCode).IsRequired().HasMaxLength(8);
            Property(x => x.RegistrationMessage).IsRequired().HasMaxLength(1000);
            Property(x => x.ResponsibleReminderControlTime).IsRequired();
            Property(x => x.ResponsibleReminderEmailList).IsRequired().HasMaxLength(400);
            Property(x => x.ShowBulletinBoardOnExtPage).IsRequired();
            Property(x => x.ShowCaseOnExternalPage).IsRequired();
            Property(x => x.ShowDashboardOnExternalPage).IsRequired();
            Property(x => x.ShowFAQOnExternalPage).IsRequired();
            Property(x => x.WorkingDayEnd).IsRequired();
            Property(x => x.WorkingDayStart).IsRequired();
            Property(x => x.ChangeTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.RegTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcustomer");
        }
    }
}
