namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ChecklistActionConfiguration : EntityTypeConfiguration<ChecklistAction>
    {
        internal ChecklistActionConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(cs => cs.ChecklistService)
                .WithMany(ca => ca.ChecklistActions)
                .HasForeignKey(cs => cs.ChecklistService_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.ChecklistService_Id).IsRequired();
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(500).HasColumnName("ChecklistAction");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblchecklistaction");
        }
    }
}

