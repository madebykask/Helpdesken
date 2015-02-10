namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseSolutionConfiguration : EntityTypeConfiguration<CaseSolution>
    {
        internal CaseSolutionConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasOptional(x => x.CaseSolutionCategory)
                .WithMany()
                .HasForeignKey(x => x.CaseSolutionCategory_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.CaseSolutionSchedule)
               .WithRequiredPrincipal()
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CaseType)
                .WithMany()
                .HasForeignKey(x => x.CaseType_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CaseWorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.CaseWorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.Category_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.FinishingCause)
                .WithMany()
                .HasForeignKey(x => x.FinishingCause_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.PerformerUser)
                .WithMany()
                .HasForeignKey(x => x.PerformerUser_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.Priority_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.ProductArea)
                .WithMany()
                .HasForeignKey(x => x.ProductArea_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.Project_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.WorkingGroup)
                .WithMany()
                .HasForeignKey(x => x.WorkingGroup_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Caption).IsRequired().HasMaxLength(60);
            this.Property(x => x.CaseSolutionCategory_Id).IsOptional();
            this.Property(x => x.CaseWorkingGroup_Id).IsOptional();
            this.Property(x => x.CaseType_Id).IsOptional();
            this.Property(x => x.Category_Id).IsOptional();
            this.Property(x => x.Department_Id).IsOptional();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.Description).IsOptional();
            this.Property(x => x.FinishingCause_Id).IsOptional();
            this.Property(x => x.Miscellaneous).IsRequired().HasMaxLength(1000);
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("CaseSolutionName");
            this.Property(x => x.NoMailToNotifier).IsRequired();
            this.Property(x => x.PerformerUser_Id).IsOptional();
            this.Property(x => x.Priority_Id).IsOptional();
            this.Property(x => x.ProductArea_Id).IsOptional();
            this.Property(x => x.Project_Id).IsOptional();
            this.Property(x => x.ReportedBy).IsOptional().HasMaxLength(40);
            this.Property(x => x.Text_External).IsRequired().HasMaxLength(1500);
            this.Property(x => x.Text_Internal).IsRequired().HasMaxLength(1000);
            this.Property(x => x.WorkingGroup_Id).IsOptional();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.TemplatePath).IsOptional();
            this.Property(x => x.ShowInSelfService).IsRequired();
            this.Property(x => x.OrderNum).IsOptional();
            this.Property(x => x.FormGUID).IsOptional();

            this.ToTable("tblcasesolution");
        }
    }
}
