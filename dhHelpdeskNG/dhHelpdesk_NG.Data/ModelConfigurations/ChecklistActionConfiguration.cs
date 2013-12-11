using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class ChecklistActionConfiguration : EntityTypeConfiguration<ChecklistAction>
    {
        internal ChecklistActionConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(cs => cs.ChecklistService)
                .WithMany(ca => ca.ChecklistActions)
                .HasForeignKey(cs => cs.ChecklistService_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.ChecklistService_Id).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(500).HasColumnName("ChecklistAction");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblchecklistaction");
        }
    }
}

