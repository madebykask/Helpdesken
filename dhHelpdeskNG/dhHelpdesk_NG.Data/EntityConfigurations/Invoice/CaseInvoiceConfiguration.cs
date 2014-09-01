namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Invoice;

    internal sealed class CaseInvoiceConfiguration : EntityTypeConfiguration<CaseInvoiceEntity>
    {
        internal CaseInvoiceConfiguration()
        {
            this.HasKey(i => i.Id);
            this.Property(i => i.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(i => i.CaseId).IsRequired();
            this.HasRequired(i => i.Case)
                .WithMany()
                .HasForeignKey(i => i.CaseId)
                .WillCascadeOnDelete(false);
            this.HasMany(o => o.Orders)
                .WithRequired(a => a.Invoice)
                .HasForeignKey(a => a.InvoiceId)
                .WillCascadeOnDelete(true);

            this.ToTable("tblCaseInvoice");   
        }
    }
}