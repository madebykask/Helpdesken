using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ImpactConfiguration : EntityTypeConfiguration<Impact>
    {
        internal ImpactConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(x => x.PriorityImpactUrgencies)
                .WithOptional(x => x.Impact)
                .HasForeignKey(x => x.Impact_Id);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Impact");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblimpact");
        }
    }
}
