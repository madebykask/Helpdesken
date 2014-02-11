namespace DH.Helpdesk.Dal.EntityConfigurations.WorkstationModules
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class LogicalDriveConfiguration : EntityTypeConfiguration<LogicalDrive>
    {
        public LogicalDriveConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Computer)
                .WithMany()
                .HasForeignKey(x => x.Computer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.DriveLetter).IsRequired().HasMaxLength(10);
            this.Property(x => x.DriveType).IsRequired();
            this.Property(x => x.FreeBytes).IsRequired();
            this.Property(x => x.TotalBytes).IsRequired();
            this.Property(x => x.FileSystemName).IsRequired().HasMaxLength(30);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblLogicalDrive");
        }
    }
}