namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class UrgencyConfiguration : EntityTypeConfiguration<Urgency>
    {
        internal UrgencyConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(x => x.PriorityImpactUrgencies)
                .WithOptional(x => x.Urgency)
                .HasForeignKey(x => x.Urgency_Id);

            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("Urgency");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblurgency");
        }
    }
}
