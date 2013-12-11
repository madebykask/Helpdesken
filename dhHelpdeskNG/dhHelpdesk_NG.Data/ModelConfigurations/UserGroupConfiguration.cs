using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class UserGroupConfiguration : EntityTypeConfiguration<UserGroup>
    {
        internal UserGroupConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("UserGroup");
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblusergroups");
        }
    }
}
