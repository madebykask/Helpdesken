namespace DH.Helpdesk.Dal.EntityConfigurations.Printers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Printers;

    public class PrinterFieldSettingsConfiguration : EntityTypeConfiguration<PrinterFieldSettings>
    {
        public PrinterFieldSettingsConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.PrinterField).IsRequired().HasMaxLength(50);
            this.Property(x => x.Show).IsRequired();
            this.Property(x => x.Label).IsRequired().HasMaxLength(50);
            this.Property(x => x.Label_ENG).IsRequired().HasMaxLength(50);
            this.Property(x => x.Required).IsRequired();
            this.Property(x => x.FieldHelp).IsRequired().HasMaxLength(200);
            this.Property(x => x.ShowInList).IsRequired();

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblPrinterFieldSettings");
        }
    }
}