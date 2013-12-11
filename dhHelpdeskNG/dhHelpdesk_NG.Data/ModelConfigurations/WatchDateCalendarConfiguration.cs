using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class WatchDateCalendarValueConfiguration : EntityTypeConfiguration<WatchDateCalendarValue>
    {
        internal WatchDateCalendarValueConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.WatchDateCalendar)
                .WithMany(x => x.WDCValues)
                .HasForeignKey(x => x.WatchDateCalendar_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.CreatedDate).IsRequired();
            Property(x => x.WatchDate).IsRequired();
            Property(x => x.WatchDateCalendar_Id).IsRequired();
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblwatchdatecalendarvalue");
        }
    }

    public class WatchDateCalendarConfiguration : EntityTypeConfiguration<WatchDateCalendar>
    {
        internal WatchDateCalendarConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.CreatedDate).IsRequired();
            Property(x => x.Name).IsRequired().HasColumnName("WatchDateCalendarName");
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblwatchdatecalendar");
        }
    }
}
