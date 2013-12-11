using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        internal CountryConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.IsDefault).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Country");
            Property(x => x.SyncChangedDate).IsOptional();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcountry");
        }
    }
}
