namespace DH.Helpdesk.Dal.EntityConfigurations.Computers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Computers;

    public class ComputerConfiguration : EntityTypeConfiguration<Computer>
    {
        public ComputerConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ComputerModel)
                .WithMany()
                .HasForeignKey(x => x.ComputerModel_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Domain)
                .WithMany()
                .HasForeignKey(x => x.Domain_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ComputerType)
                .WithMany()
                .HasForeignKey(x => x.ComputerType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.OS)
                .WithMany()
                .HasForeignKey(x => x.OS_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Processor)
                .WithMany()
                .HasForeignKey(x => x.Processor_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.RAM)
                .WithMany()
                .HasForeignKey(x => x.RAM_ID)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.NIC)
                .WithMany()
                .HasForeignKey(x => x.NIC_ID)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Room)
                .WithMany()
                .HasForeignKey(x => x.Room_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.Department_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.OU)
                .WithMany()
                .HasForeignKey(x => x.OU_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Region)
                .WithMany()
                .HasForeignKey(x => x.Region_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ContractStatus)
                .WithMany()
                .HasForeignKey(x => x.ContractStatus_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ComputerGUID).IsRequired().HasMaxLength(100);
            this.Property(x => x.ComputerModelName).HasColumnName("ComputerModel").IsRequired().HasMaxLength(50);
            this.Property(x => x.ComputerName).IsRequired().HasMaxLength(60);
            this.Property(x => x.Manufacturer).IsRequired().HasMaxLength(50);
            this.Property(x => x.SerialNumber).IsRequired().HasMaxLength(60);
            this.Property(x => x.BIOSVersion).IsRequired().HasMaxLength(40);
            this.Property(x => x.BIOSDate).IsOptional();
            this.Property(x => x.TheftMark).IsRequired().HasMaxLength(50);
            this.Property(x => x.CarePackNumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.ChassisType).IsRequired().HasMaxLength(25);
            this.Property(x => x.Location).IsRequired().HasMaxLength(100);
            this.Property(x => x.BarCode).IsRequired().HasMaxLength(20);
            this.Property(x => x.PurchaseDate).IsOptional();
            this.Property(x => x.SP).IsRequired().HasMaxLength(50);
            this.Property(x => x.Version).IsRequired().HasMaxLength(20);
            this.Property(x => x.ProcessorInfo).IsOptional().HasMaxLength(20);
            this.Property(x => x.IPAddress).IsRequired().HasMaxLength(50);
            this.Property(x => x.MACAddress).IsRequired().HasMaxLength(20);
            this.Property(x => x.RAS).IsRequired();

            this.Property(x => x.NovellClient).IsRequired().HasMaxLength(100);
            this.Property(x => x.HardDrive).HasColumnName("Harddrive").IsRequired().HasMaxLength(50);
            this.Property(x => x.VideoCard).IsRequired().HasMaxLength(100);
            this.Property(x => x.SoundCard).IsRequired().HasMaxLength(100);
            this.Property(x => x.MonitorModel).IsRequired().HasMaxLength(50);
            this.Property(x => x.MonitorSerialnumber).IsRequired().HasMaxLength(50);
            this.Property(x => x.MonitorTheftMark).HasColumnName("MonitortheftMark").IsRequired().HasMaxLength(50);

            this.Property(x => x.ContractStartDate).IsOptional();
            this.Property(x => x.ContractEndDate).IsOptional();
            this.Property(x => x.ContractStatus_Id).HasColumnName("ComputerContractStatus_Id").IsOptional();
            this.Property(x => x.ContractNumber).IsOptional().HasMaxLength(50);
            this.Property(x => x.Price).IsRequired();
            this.Property(x => x.ComputerFileName).IsOptional();
            this.Property(x => x.ComputerDocument).IsOptional();
            this.Property(x => x.Info).IsOptional().HasMaxLength(1000);
            this.Property(x => x.LoggedUser).IsRequired().HasMaxLength(255);

            this.Property(x => x.Status).IsOptional();
            this.Property(x => x.Stolen).IsRequired();
            this.Property(x => x.ReplacedWithComputerName).IsRequired().HasMaxLength(20);
            this.Property(x => x.SendBack).IsRequired();
            this.Property(x => x.ScrapDate).IsOptional();
            this.Property(x => x.ComputerRole).IsRequired();
            this.Property(x => x.LDAPPath).IsRequired().HasMaxLength(200);
            this.Property(x => x.RegistrationCode).IsOptional().HasMaxLength(30);
            this.Property(x => x.ProductKey).IsOptional().HasMaxLength(30);
            this.Property(x => x.ScanDate).IsOptional();
            this.Property(x => x.Updated).IsRequired();
            this.Property(x => x.SyncChangedDate).IsOptional();
            this.Property(x => x.WarrantyEndDate).IsOptional();
            this.Property(x => x.RegUser_Id).IsOptional();
            this.Property(x => x.ChangedByUser_Id).IsOptional();

            this.Property(x => x.Domain_Id).IsOptional();
            this.Property(x => x.Department_Id).IsOptional();
            this.Property(x => x.OU_Id).IsOptional();
            this.Property(x => x.Region_Id).IsOptional();

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.Property(x => x.ContactName).IsOptional().HasMaxLength(50);
            this.Property(x => x.ContactPhone).IsOptional().HasMaxLength(50);
            this.Property(x => x.ContactEmailAddress).HasColumnName("ContactEmail").IsOptional().HasMaxLength(50);
            this.Property(x => x.LocationAddress).IsOptional().HasMaxLength(50);
            this.Property(x => x.LocationPostalCode).IsOptional().HasMaxLength(10);
            this.Property(x => x.LocationPostalAddress).IsOptional().HasMaxLength(50);
            this.Property(x => x.LocationRoom).IsOptional().HasMaxLength(100);
            this.Property(x => x.Location2).IsOptional().HasMaxLength(100);

            this.ToTable("tblComputer");
        }
    }
}
