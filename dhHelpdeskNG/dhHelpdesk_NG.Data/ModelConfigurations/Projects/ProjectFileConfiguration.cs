namespace dhHelpdesk_NG.Data.ModelConfigurations.Projects
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Projects;

    public sealed class ProjectFileConfiguration : EntityTypeConfiguration<ProjectFile>
    {
        internal ProjectFileConfiguration()
        {
            this.HasKey(f => f.Id);

            this.Property(f => f.Project_Id).IsRequired();

            this.HasRequired(f => f.Project)
                .WithMany(f => f.ProjectFiles)
                .HasForeignKey(f => f.Project_Id)
                .WillCascadeOnDelete(false);

            this.Property(f => f.FileName).IsRequired().HasMaxLength(200);
            this.Property(f => f.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.Property(f => f.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.ToTable("tblprojectfile");
        }
    }
}