namespace dhHelpdesk_NG.Data.ModelConfigurations.Projects
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;
    using dhHelpdesk_NG.Domain.Projects;

    public class ProjectLogConfiguration : EntityTypeConfiguration<ProjectLog>
    {
        public ProjectLogConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.Project_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.LogDate).IsOptional();
            this.Property(x => x.ChangeDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.LogText).IsRequired();
            this.Property(x => x.User_Id).IsRequired();

            this.ToTable("tblprojectlog");
        }
    }
}
