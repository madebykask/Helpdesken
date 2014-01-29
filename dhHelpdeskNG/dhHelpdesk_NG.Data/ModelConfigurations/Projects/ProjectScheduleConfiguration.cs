namespace dhHelpdesk_NG.Data.ModelConfigurations.Projects
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using dhHelpdesk_NG.Domain.Projects;

    public class ProjectScheduleConfiguration : EntityTypeConfiguration<ProjectSchedule>
    {
        public ProjectScheduleConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.Project_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.TimeType)
                .WithMany()
                .HasForeignKey(x => x.TimeType_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Parent_ProjectSchedule_Id).IsOptional();
            this.Property(x => x.FinishDate).IsOptional();
            this.Property(x => x.ScheduleDate).IsOptional();
            this.Property(x => x.User_Id).IsOptional();
            this.Property(x => x.TimeType_Id).IsOptional();
            this.Property(x => x.CaseNumber).IsOptional();
            this.Property(x => x.CalculatedDate).IsOptional();

            this.Property(x => x.Note).IsRequired();
            this.Property(x => x.Pos).IsRequired();
            this.Property(x => x.IsActive).HasColumnName("Status").IsRequired();
            this.Property(x => x.CalculatedTime);
            this.Property(x => x.Activity);
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblprojectschedule");
        }
    }
}
