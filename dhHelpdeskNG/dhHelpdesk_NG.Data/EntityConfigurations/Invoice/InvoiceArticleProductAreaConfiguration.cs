namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Invoice;

    internal sealed class InvoiceArticleProductAreaConfiguration : EntityTypeConfiguration<InvoiceArticleProductAreaEntity>
    {
        internal InvoiceArticleProductAreaConfiguration()
        {
            this.HasKey(ap => new { ap.InvoiceArticle_Id, ap.ProductArea_Id });

            this.ToTable("tblInvoiceArticle_tblProductArea");
        }
    }
}