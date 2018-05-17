namespace DH.Helpdesk.Dal.EntityConfigurations.Servers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Servers;

    public class ServerConfiguration : EntityTypeConfiguration<Server>
    {
        public ServerConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.OperatingSystem)
                .WithMany()
                .HasForeignKey(x => x.OperatingSystem_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Processor)
                .WithMany()
                .HasForeignKey(x => x.Processor_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.RAM)
                .WithMany()
                .HasForeignKey(x => x.RAM_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.NIC)
                .WithMany()
                .HasForeignKey(x => x.NIC_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Room)
                .WithMany()
                .HasForeignKey(x => x.Room_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ServerName).IsRequired().HasMaxLength(60);
            this.Property(x => x.ServerDescription).IsRequired().HasMaxLength(200);
            this.Property(x => x.ServerModel).IsRequired().HasMaxLength(50);
            this.Property(x => x.Manufacturer).IsRequired().HasMaxLength(100);
            this.Property(x => x.SerialNumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.ChassisType).IsRequired().HasMaxLength(25);
            this.Property(x => x.Location).IsRequired().HasMaxLength(50);
            this.Property(x => x.BarCode).IsRequired().HasMaxLength(20);
            this.Property(x => x.PurchaseDate).IsOptional();
            this.Property(x => x.SP).IsRequired().HasMaxLength(50);
            this.Property(x => x.Version).IsRequired().HasMaxLength(20);
            this.Property(x => x.IPAddress).IsRequired().HasMaxLength(20);
            this.Property(x => x.MACAddress).IsRequired().HasMaxLength(20);
            this.Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            this.Property(x => x.URL).IsRequired().HasMaxLength(200);
            this.Property(x => x.URL2).IsRequired().HasMaxLength(100);
            this.Property(x => x.Owner).IsRequired().HasMaxLength(50);

            this.Property(x => x.Info).IsOptional().HasMaxLength(2000);
            this.Property(x => x.RegistrationCode).IsOptional().HasMaxLength(30);
            this.Property(x => x.ProductKey).IsOptional().HasMaxLength(30);
            this.Property(x => x.ChangedByUser_Id).IsOptional();
            this.Property(x => x.SyncChangedDate).IsOptional();

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblServer");
        }
    }
}
