using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        internal DepartmentConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(d => d.Country)
                .WithMany()
                .HasForeignKey(d => d.Country_Id)
                .WillCascadeOnDelete(false);

            HasOptional(d => d.Region)
                .WithMany(r => r.Departments)
                .HasForeignKey(d => d.Region_Id)
                .WillCascadeOnDelete(false);

            HasOptional(d => d.HolidayHeader)
                .WithMany()
                .HasForeignKey(d => d.HolidayHeader_Id)
                .WillCascadeOnDelete(false);

            HasOptional(d => d.WatchDateCalendar)
                .WithMany()
                .HasForeignKey(d => d.WatchDateCalendar_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.AccountancyAmount).IsRequired();
            Property(x => x.Charge).IsRequired();
            Property(x => x.ChargeMandatory).IsRequired();
            Property(x => x.Country_Id).IsOptional();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.DepartmentId).IsRequired().HasMaxLength(20);
            Property(x => x.HeadOfDepartment).IsRequired().HasMaxLength(50);
            Property(x => x.HeadOfDepartmentEMail).IsRequired().HasMaxLength(50);
            Property(x => x.HomeDirectory).IsOptional().HasMaxLength(200);
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.IsEMailDefault).IsRequired().HasColumnName("isEmailDefault");
            Property(x => x.DepartmentName).IsRequired().HasMaxLength(50).HasColumnName("Department");
            Property(x => x.Path).IsRequired().HasMaxLength(1000).HasColumnName("NDSPath");
            Property(x => x.Region_Id).IsOptional();
            Property(x => x.SearchKey).IsRequired().HasMaxLength(50);
            Property(x => x.ShowInvoice).IsRequired();
            Property(x => x.SyncChangedDate).IsOptional();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.HolidayHeader_Id).IsOptional();
            Property(x => x.WatchDateCalendar_Id).IsOptional();

            ToTable("tbldepartment");
        }
    }
}
