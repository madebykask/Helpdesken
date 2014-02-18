namespace DH.Helpdesk.Dal.EntityConfigurations.Servers
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Servers;

    public class ServerSoftwareConfiguration : EntityTypeConfiguration<ServerSoftware>
    {
        public ServerSoftwareConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Server)
                .WithMany()
                .HasForeignKey(x => x.Server_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Name).IsRequired().HasMaxLength(100);
            this.Property(x => x.Version).IsRequired().HasMaxLength(50);
            this.Property(x => x.Manufacturer).IsRequired().HasMaxLength(100);
            this.Property(x => x.Registration_code).IsRequired().HasMaxLength(50);
            this.Property(x => x.Product_key).IsOptional().HasMaxLength(30);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblServerSoftware");
        }
    }
}