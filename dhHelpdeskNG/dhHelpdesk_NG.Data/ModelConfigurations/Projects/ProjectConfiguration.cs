namespace dhHelpdesk_NG.Data.ModelConfigurations.Projects
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Projects;

    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        internal ProjectConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Manager)
                .WithMany()
                .HasForeignKey(x => x.ProjectManager)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Customer_Id).IsRequired().HasColumnName("Customer_id");
            this.Property(x => x.Description).IsRequired().HasMaxLength(2000);
            this.Property(x => x.IsActive).IsRequired().HasColumnName("Status");
            this.Property(x => x.Name).IsRequired().HasMaxLength(50);
            this.Property(x => x.ProjectManager).IsOptional();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.EndDate).HasColumnName("FinishDate").IsOptional();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblproject");
        }
    }
}
