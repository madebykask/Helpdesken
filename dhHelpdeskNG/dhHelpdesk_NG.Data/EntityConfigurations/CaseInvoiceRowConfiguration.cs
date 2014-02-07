namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseInvoiceRowConfiguration : EntityTypeConfiguration<CaseInvoiceRow>
    {
        internal CaseInvoiceRowConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(c => c.Case)
                .WithMany(c => c.CaseInvoiceRows)
                .HasForeignKey(c => c.Case_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Charge).IsRequired();
            this.Property(x => x.InvoiceNumber).IsOptional().HasMaxLength(50);
            this.Property(x => x.InvoicePrice).IsRequired();
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcaseinvoicerow");
        }
    }
}
