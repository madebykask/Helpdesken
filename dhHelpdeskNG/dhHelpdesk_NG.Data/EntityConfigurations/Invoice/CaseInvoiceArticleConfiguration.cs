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
            this.Property(a => a.OrderId).IsRequired();
            this.HasRequired(a => a.Order)
                .WithMany()
                .HasForeignKey(a => a.OrderId)
                .WillCascadeOnDelete(false);
            this.Property(a => a.ArticleId).IsOptional();
            this.HasRequired(a => a.Article)
                .WithMany()
                .HasForeignKey(a => a.ArticleId)
                .WillCascadeOnDelete(false);
            this.Property(a => a.Name).IsRequired().HasMaxLength(100);
            this.Property(a => a.Amount).IsOptional();
            this.Property(a => a.Ppu).IsOptional();
            this.Property(a => a.Position).IsRequired();
            //this.Property(a => a.IsInvoiced).IsRequired();

            this.ToTable("tblCaseInvoiceArticle");            
        }
    }
}