using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CaseSolutionScheduleConfiguration : EntityTypeConfiguration<CaseSolutionSchedule>
    {
        internal CaseSolutionScheduleConfiguration()
        {
            HasKey(x => x.CaseSolution_Id);

            //HasRequired(x => x.CaseSolution)
            //    .WithRequiredDependent();

            Property(x => x.ScheduleDay).IsOptional().HasMaxLength(50);
            Property(x => x.ScheduleTime).IsRequired();
            Property(x => x.ScheduleType).IsRequired();
            Property(x => x.ScheduleWatchDate).IsRequired();
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            ToTable("tblcasesolutionschedule");
        }
    }
}
