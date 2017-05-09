namespace DH.Helpdesk.Dal.EntityConfigurations.Invoice
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Invoice;

    internal sealed class CaseInvoiceSettingsConfiguration : EntityTypeConfiguration<CaseInvoiceSettingsEntity>
    {
        internal CaseInvoiceSettingsConfiguration()
        {
            this.HasKey(s => s.Id);
            this.Property(s => s.CustomerId).IsRequired();
            this.Property(s => s.ExportPath).IsOptional().HasMaxLength(200);
            this.Property(s => s.Currency).IsOptional().HasMaxLength(50);
            this.Property(s => s.DocTemplate).IsOptional().HasMaxLength(50);
            this.Property(s => s.Issuer).IsOptional().HasMaxLength(50);
            this.Property(s => s.OrderNoPrefix).IsOptional().HasMaxLength(50);
            this.Property(s => s.OurReference).IsOptional().HasMaxLength(50);
            this.Property(s => s.Filter).IsOptional().HasMaxLength(50);

            this.ToTable("tblCaseInvoiceSettings");
        }
    }
}