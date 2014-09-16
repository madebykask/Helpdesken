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

            this.ToTable("tblCaseInvoiceSettings");
        }
    }
}