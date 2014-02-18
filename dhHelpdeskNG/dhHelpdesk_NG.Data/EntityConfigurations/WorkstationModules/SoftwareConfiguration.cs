namespace DH.Helpdesk.Dal.EntityConfigurations.WorkstationModules
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class SoftwareConfiguration : EntityTypeConfiguration<Software>
    {
        public SoftwareConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Computer)
                .WithMany()
                .HasForeignKey(x => x.Computer_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Name).IsRequired().HasMaxLength(100);
            this.Property(x => x.Version).IsRequired().HasMaxLength(100);
            this.Property(x => x.Manufacturer).IsRequired().HasMaxLength(50);
            this.Property(x => x.Registration_code).IsRequired().HasMaxLength(50);
            this.Property(x => x.Product_key).IsOptional().HasMaxLength(30);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.ChangedDate).IsRequired();

            this.ToTable("tblSoftware");
        }
    }
}
