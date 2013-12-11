using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class StateSecondaryConfiguration : EntityTypeConfiguration<StateSecondary>
    {
        internal StateSecondaryConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id);

            HasOptional(x => x.WorkingGroup)
               .WithMany()
               .HasForeignKey(x => x.WorkingGroup_Id)
               .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.IncludeInCaseStatistics).IsRequired();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("StateSecondary");
            Property(x => x.NoMailToNotifier).IsRequired();
            Property(x => x.ResetOnExternalUpdate).IsRequired();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.WorkingGroup_Id).IsOptional();

            ToTable("tblstatesecondary");
        }
    }
}
