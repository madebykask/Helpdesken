namespace DH.Helpdesk.Dal.EntityConfigurations.Printers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Printers;

    public class PrinterConfiguration : EntityTypeConfiguration<Printer>
    {
        public PrinterConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Room)
                .WithMany()
                .HasForeignKey(x => x.Room_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.Department_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.PrinterName).IsRequired().HasMaxLength(60);
            this.Property(x => x.PrinterType).IsRequired().HasMaxLength(50);
            this.Property(x => x.PrinterServer).IsRequired().HasMaxLength(50);
            this.Property(x => x.NumberOfTrays).IsRequired().HasMaxLength(20);
            this.Property(x => x.DriverName).IsRequired().HasMaxLength(50);
            this.Property(x => x.URL).IsRequired().HasMaxLength(200);
            this.Property(x => x.OU).IsRequired().HasMaxLength(50);
            this.Property(x => x.Theftmark).IsRequired().HasMaxLength(50);

            this.Property(x => x.Manufacturer).IsRequired().HasMaxLength(50);
            this.Property(x => x.SerialNumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.Location).IsRequired().HasMaxLength(100);
            this.Property(x => x.BarCode).IsRequired().HasMaxLength(20);
            this.Property(x => x.PurchaseDate).IsOptional();
            this.Property(x => x.IPAddress).IsRequired().HasMaxLength(20);
            this.Property(x => x.MACAddress).IsRequired().HasMaxLength(50);
            this.Property(x => x.Info).IsOptional().HasMaxLength(2000);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblPrinter");
        }
    }
}