using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class UrgencyConfiguration : EntityTypeConfiguration<Urgency>
    {
        internal UrgencyConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasMany(x => x.PriorityImpactUrgencies)
                .WithOptional(x => x.Urgency)
                .HasForeignKey(x => x.Urgency_Id);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Urgency");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblurgency");
        }
    }
}
