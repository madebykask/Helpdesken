namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CaseSolutionScheduleConfiguration : EntityTypeConfiguration<CaseSolutionSchedule>
    {
        internal CaseSolutionScheduleConfiguration()
        {
            this.HasKey(x => x.CaseSolution_Id);

            //HasRequired(x => x.CaseSolution)
            //    .WithRequiredDependent();

            this.Property(x => x.ScheduleDay).IsOptional().HasMaxLength(50);
            this.Property(x => x.ScheduleTime).IsRequired();
            this.Property(x => x.ScheduleType).IsRequired();
            this.Property(x => x.ScheduleWatchDate).IsRequired();
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            this.ToTable("tblcasesolutionschedule");
        }
    }
}
