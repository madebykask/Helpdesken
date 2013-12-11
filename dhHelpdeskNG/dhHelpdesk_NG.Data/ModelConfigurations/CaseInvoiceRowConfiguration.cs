using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseInvoiceRowConfiguration : EntityTypeConfiguration<CaseInvoiceRow>
    {
        internal CaseInvoiceRowConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(c => c.Case)
                .WithMany(c => c.CaseInvoiceRows)
                .HasForeignKey(c => c.Case_Id)
                .WillCascadeOnDelete(false);

            HasRequired(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUser_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Charge).IsRequired();
            Property(x => x.InvoiceNumber).IsOptional().HasMaxLength(50);
            Property(x => x.InvoicePrice).IsRequired();
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcaseinvoicerow");
        }
    }
}
