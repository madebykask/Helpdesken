namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Invoice;

    internal sealed class InvoiceArticleConfiguration : EntityTypeConfiguration<InvoiceArticleEntity>
    {
        internal InvoiceArticleConfiguration()
        {
            this.HasKey(a => a.Id);
            this.Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(a => a.ParentId).IsOptional();
            this.HasOptional(a => a.Parent)
                .WithMany()
                .HasForeignKey(a => a.ParentId)
                .WillCascadeOnDelete(false);
            this.Property(a => a.Number).IsRequired().HasMaxLength(15);
            this.Property(a => a.Name).IsRequired().HasMaxLength(100);
            this.Property(a => a.NameEng).IsRequired().HasMaxLength(100);
            this.Property(a => a.Description).IsOptional().HasMaxLength(200);
            this.Property(a => a.UnitId).IsOptional();
            this.HasOptional(a => a.Unit)
                .WithMany()
                .HasForeignKey(a => a.UnitId)
                .WillCascadeOnDelete(false);
            this.Property(a => a.Ppu).IsOptional();
            
            this.HasMany(p => p.ProductAreas)
                .WithMany(a => a.InvoiceArticles)
                .Map(m =>
                {
                    m.MapLeftKey("InvoiceArticle_Id");
                    m.MapRightKey("ProductArea_id");
                    m.ToTable("tblInvoiceArticle_tblProductArea");
                });

            //this.Property(a => a.ProductAreaId).IsRequired();
            //this.HasRequired(a => a.ProductArea)
            //    .WithMany()
            //    .HasForeignKey(a => a.ProductAreaId)
            //    .WillCascadeOnDelete(false);
            this.Property(a => a.CustomerId).IsRequired();
            this.HasRequired(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .WillCascadeOnDelete(false);

            this.ToTable("tblInvoiceArticle");            
        }
    }
}