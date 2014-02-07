namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        internal CountryConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.IsDefault).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Country");
            this.Property(x => x.SyncChangedDate).IsOptional();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcountry");
        }
    }
}
