using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ApplicationConfiguration : EntityTypeConfiguration<Application>
    {
        internal ApplicationConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Name).IsRequired().HasMaxLength(100).HasColumnName("Application");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblapplication");
        }
    }
}
