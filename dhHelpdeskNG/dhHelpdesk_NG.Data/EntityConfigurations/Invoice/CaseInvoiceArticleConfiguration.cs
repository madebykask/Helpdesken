namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Invoice;

    internal sealed class CaseInvoiceArticleConfiguration : EntityTypeConfiguration<CaseInvoiceArticleEntity>
    {
        internal CaseInvoiceArticleConfiguration()
        {
            this.HasKey(a => a.Id);
            this.Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(a => a.CaseId).IsRequired();
            this.HasRequired(a => a.Case)
                .WithMany()
                .HasForeignKey(a => a.CaseId)
                .WillCascadeOnDelete(false);
            this.Property(a => a.Number).IsOptional();
            this.Property(a => a.Name).IsRequired().HasMaxLength(100);
            this.Property(a => a.Amount).IsOptional();
            this.Property(a => a.UnitId).IsOptional();
            this.HasRequired(a => a.Unit)
                .WithMany()
                .HasForeignKey(a => a.UnitId)
                .WillCascadeOnDelete(false);
            this.Property(a => a.Ppu).IsOptional();
            this.Property(a => a.Position).IsRequired();
            this.Property(a => a.IsInvoiced).IsRequired();

            this.ToTable("tblCaseInvoiceArticle");            
        }
    }
}