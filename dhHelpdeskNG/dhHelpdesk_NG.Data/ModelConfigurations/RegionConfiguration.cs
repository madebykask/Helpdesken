using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class RegionConfiguration : EntityTypeConfiguration<Region>
    {
        internal RegionConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.IsDefault).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Region");
            Property(x => x.SearchKey).IsOptional().HasMaxLength(20);
            //Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblregion");
        }
    }
}
