namespace DH.Helpdesk.Dal.EntityConfigurations.Servers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Servers;

    public class ServerLogicalDriveConfiguration : EntityTypeConfiguration<ServerLogicalDrive>
    {
        public ServerLogicalDriveConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Server)
                .WithMany()
                .HasForeignKey(x => x.Server_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.DriveLetter).IsRequired().HasMaxLength(10);
            this.Property(x => x.DriveType).IsRequired();
            this.Property(x => x.FreeBytes).IsRequired();
            this.Property(x => x.TotalBytes).IsRequired();
            this.Property(x => x.FileSystemName).IsRequired().HasMaxLength(30);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblServerLogicalDrive");
        }
    }
}