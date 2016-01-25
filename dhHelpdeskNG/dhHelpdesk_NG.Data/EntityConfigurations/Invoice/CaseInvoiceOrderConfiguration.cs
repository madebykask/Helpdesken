namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Invoice;

    internal sealed class CaseInvoiceOrderConfiguration : EntityTypeConfiguration<CaseInvoiceOrderEntity>
    {
        internal CaseInvoiceOrderConfiguration()
        {
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.InvoiceId).IsRequired();
            this.HasRequired(o => o.Invoice)
                .WithMany()
                .HasForeignKey(o => o.InvoiceId)
                .WillCascadeOnDelete(false);
            this.Property(o => o.Number).IsRequired();
            this.Property(o => o.Date).IsRequired();
            this.HasMany(o => o.Articles)
                .WithRequired(a => a.Order)
                .HasForeignKey(a => a.OrderId)
                .WillCascadeOnDelete(true);

            this.ToTable("tblCaseInvoiceOrder");   
        }
    }
}