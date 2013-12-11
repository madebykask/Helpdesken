using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class PriorityImpactUrgencyConfiguration : EntityTypeConfiguration<PriorityImpactUrgency>
    {
        internal PriorityImpactUrgencyConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.Impact)
                .WithMany()
                .HasForeignKey(x => x.Impact_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.Priority_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Urgency)
                .WithMany()
                .HasForeignKey(x => x.Urgency_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Impact_Id).IsOptional();
            Property(x => x.Priority_Id).IsRequired();
            Property(x => x.Urgency_Id).IsOptional();
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblpriority_impact_urgency");
        }
    }
}
