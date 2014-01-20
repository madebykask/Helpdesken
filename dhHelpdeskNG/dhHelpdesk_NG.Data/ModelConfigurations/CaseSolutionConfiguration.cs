using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseSolutionConfiguration : EntityTypeConfiguration<CaseSolution>
    {
        internal CaseSolutionConfiguration()
        {
            HasKey(x => x.Id);

            HasOptional(x => x.CaseSolutionCategory)
                .WithMany()
                .HasForeignKey(x => x.CaseSolutionCategory_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.CaseSolutionSchedule)
               .WithRequiredPrincipal()
               .WillCascadeOnDelete(false);

            HasOptional(x => x.CaseType)
                .WithMany()
                .HasForeignKey(x => x.CaseType_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.CaseWorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.CaseWorkingGroup_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.Category_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            HasOptional(x => x.FinishingCause)
                .WithMany()
                .HasForeignKey(x => x.FinishingCause_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.PerformerUser)
                .WithMany()
                .HasForeignKey(x => x.PerformerUser_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.Priority_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.ProductArea)
                .WithMany()
                .HasForeignKey(x => x.ProductArea_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.Project_Id)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.WorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.Caption).IsRequired().HasMaxLength(60);
            Property(x => x.CaseSolutionCategory_Id).IsOptional();
            Property(x => x.CaseWorkingGroup_Id).IsOptional();
            Property(x => x.CaseType_Id).IsOptional();
            Property(x => x.Category_Id).IsOptional();
            Property(x => x.Department_Id).IsOptional();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.Description).IsOptional();
            Property(x => x.FinishingCause_Id).IsOptional();
            Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("CaseSolutionName");
            Property(x => x.NoMailToNotifier).IsRequired();
            Property(x => x.PerformerUser_Id).IsOptional();
            Property(x => x.Priority_Id).IsOptional();
            Property(x => x.ProductArea_Id).IsOptional();
            Property(x => x.Project_Id).IsOptional();
            Property(x => x.ReportedBy).IsOptional().HasMaxLength(40);
            Property(x => x.Text_External).IsRequired().HasMaxLength(1500);
            Property(x => x.Text_Internal).IsRequired().HasMaxLength(1000);
            Property(x => x.WorkingGroup_Id).IsOptional();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcasesolution");
        }
    }
}
