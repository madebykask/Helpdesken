namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        internal DepartmentConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(d => d.Country)
                .WithMany()
                .HasForeignKey(d => d.Country_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(d => d.Region)
                .WithMany(r => r.Departments)
                .HasForeignKey(d => d.Region_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(d => d.HolidayHeader)
                .WithMany()
                .HasForeignKey(d => d.HolidayHeader_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(d => d.WatchDateCalendar)
                .WithMany()
                .HasForeignKey(d => d.WatchDateCalendar_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.AccountancyAmount).IsRequired();
            this.Property(x => x.Charge).IsRequired();
            this.Property(x => x.ChargeMandatory).IsRequired();
            this.Property(x => x.Country_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.DepartmentId).IsRequired().HasMaxLength(20);
            this.Property(x => x.HeadOfDepartment).IsRequired().HasMaxLength(50);
            this.Property(x => x.HeadOfDepartmentEMail).IsRequired().HasMaxLength(50);
            this.Property(x => x.HomeDirectory).IsOptional().HasMaxLength(200);
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsEMailDefault).IsRequired().HasColumnName("isEmailDefault");
            this.Property(x => x.DepartmentName).IsRequired().HasMaxLength(200).HasColumnName("Department");
            this.Property(x => x.Path).IsRequired().HasMaxLength(1000).HasColumnName("NDSPath");
            this.Property(x => x.Region_Id).IsOptional();
            this.Property(x => x.SearchKey).IsRequired().HasMaxLength(400);
            this.Property(x => x.ShowInvoice).IsRequired();
            this.Property(x => x.SyncChangedDate).IsOptional();
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.HolidayHeader_Id).IsOptional();
            this.Property(x => x.WatchDateCalendar_Id).IsOptional();
            this.Property(x => x.OverTimeAmount).IsRequired();
            this.Property(x => x.Code).IsOptional().HasMaxLength(20);
            this.Property(x => x.DepartmentGUID).IsOptional();
            this.Property(x => x.LanguageId).IsOptional();
            //this.Property(x => x.SynchronizedDate).IsOptional();
            this.Property(x => x.InvoiceChargeType).IsRequired();
            this.Property(x => x.ShowInvoiceTime).IsRequired();
            this.Property(x => x.ShowInvoiceOvertime).IsRequired();
            this.Property(x => x.ShowInvoiceMaterial).IsRequired();
            this.Property(x => x.ShowInvoicePrice).IsRequired();
            this.Property(x => x.DisabledForOrder).IsRequired();

            this.ToTable("tbldepartment");
        }
    }
}
