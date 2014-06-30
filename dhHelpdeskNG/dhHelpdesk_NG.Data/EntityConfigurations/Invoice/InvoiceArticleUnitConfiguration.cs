namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Invoice;

    internal sealed class InvoiceArticleUnitConfiguration : EntityTypeConfiguration<InvoiceArticleUnitEntity>
    {
        internal InvoiceArticleUnitConfiguration()
        {
            this.HasKey(u => u.Id);
            this.Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(u => u.Name).IsRequired().HasMaxLength(20);
            this.Property(u => u.CustomerId).IsRequired();

            this.ToTable("tblInvoiceArticleUnit");
        }
    }
}