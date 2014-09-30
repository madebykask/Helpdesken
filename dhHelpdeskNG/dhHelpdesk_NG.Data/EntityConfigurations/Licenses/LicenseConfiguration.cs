namespace DH.Helpdesk.Dal.EntityConfigurations.Licenses
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    internal sealed class LicenseConfiguration : EntityTypeConfiguration<License>
    {
        internal LicenseConfiguration()
        {
            this.HasKey(l => l.Id);
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(l => l.LicenseNumber).IsRequired().HasMaxLength(100);
            this.Property(l => l.NumberOfLicenses).IsRequired();
            this.Property(l => l.PurshaseDate).IsOptional();
            this.Property(l => l.Price).IsRequired();
            this.Property(l => l.PurshaseInfo).IsOptional().HasMaxLength(200);
            this.Property(l => l.PriceYear).IsRequired();
            this.Property(l => l.UpgradeLicense_Id);
            this.Property(l => l.ValidDate).IsOptional();
            this.Property(l => l.Info).IsRequired().HasMaxLength(1000);
            this.Property(l => l.ChangedDate).IsRequired();
            this.Property(l => l.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            
            this.HasRequired(l => l.Product)
                .WithMany()
                .HasForeignKey(l => l.Product_Id);

            this.HasRequired(l => l.Vendor)
                .WithMany()
                .HasForeignKey(l => l.Vendor_Id);

            this.HasRequired(l => l.Region)
                .WithMany()
                .HasForeignKey(l => l.Region_Id);

            this.HasRequired(l => l.Department)
                .WithMany()
                .HasForeignKey(l => l.Department_Id);

            this.ToTable("tblLicense");       
        }
    }
}