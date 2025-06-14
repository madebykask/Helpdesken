namespace DH.Helpdesk.Dal.EntityConfigurations.Projects
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Projects;

    public class ProjectCollaboratorConfiguration : EntityTypeConfiguration<ProjectCollaborator>
    {
        public ProjectCollaboratorConfiguration()
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

            this.Property(x => x.User_Id).IsRequired();

            this.ToTable("tblprojectcollaborator");
        }
    }
}