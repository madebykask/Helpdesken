namespace DH.Helpdesk.Dal.EntityConfigurations.Licenses
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    internal sealed class LicenseFileConfiguration : EntityTypeConfiguration<LicenseFile>
    {
        internal LicenseFileConfiguration()
        {
            this.HasKey(f => f.Id);
            this.Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(f => f.FileName).IsRequired().HasMaxLength(200);
            this.Property(f => f.File).IsOptional();
            this.Property(f => f.CreatedDate).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblLicenseFile");   
        }
    }
}