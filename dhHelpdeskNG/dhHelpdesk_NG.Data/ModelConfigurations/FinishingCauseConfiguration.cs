using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class FinishingCauseConfiguration : EntityTypeConfiguration<FinishingCause>
    {
        internal FinishingCauseConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.Customer_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.FinishingCauseCategory)
                .WithMany()
                .HasForeignKey(x => x.FinishingCauseCategory_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.ParentFinishingCause)
                .WithMany(x => x.SubFinishingCauses)
                .HasForeignKey(x => x.Parent_FinishingCause_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.FinishingCauseCategory_Id).IsOptional();
            Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("FinishingCause");
            Property(x => x.Parent_FinishingCause_Id).IsOptional();
            Property(x => x.PromptUser).IsRequired();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblfinishingcause");
        }
    }
}
