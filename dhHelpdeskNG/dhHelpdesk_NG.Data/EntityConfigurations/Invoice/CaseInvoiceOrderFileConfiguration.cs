namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Invoice;

    internal sealed class CaseInvoiceOrderFileConfiguration : EntityTypeConfiguration<CaseInvoiceOrderFileEntity>
    {
        internal CaseInvoiceOrderFileConfiguration()
        {
            this.HasKey(f => f.Id);
            this.Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasRequired(f => f.Order)
                .WithMany(o => o.Files)
                .HasForeignKey(f => f.OrderId);
            this.Property(f => f.FileName).IsRequired().HasMaxLength(200);
            this.Property(f => f.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblCaseInvoiceOrderFile");   
        }
    }
}