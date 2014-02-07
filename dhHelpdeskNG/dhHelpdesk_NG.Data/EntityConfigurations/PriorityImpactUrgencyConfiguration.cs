namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class PriorityImpactUrgencyConfiguration : EntityTypeConfiguration<PriorityImpactUrgency>
    {
        internal PriorityImpactUrgencyConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.Impact)
                .WithMany()
                .HasForeignKey(x => x.Impact_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.Priority_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Urgency)
                .WithMany()
                .HasForeignKey(x => x.Urgency_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Impact_Id).IsOptional();
            this.Property(x => x.Priority_Id).IsRequired();
            this.Property(x => x.Urgency_Id).IsOptional();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblpriority_impact_urgency");
        }
    }
}
