namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class WatchDateCalendarValueConfiguration : EntityTypeConfiguration<WatchDateCalendarValue>
    {
        internal WatchDateCalendarValueConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.WatchDateCalendar)
                .WithMany(x => x.WDCValues)
                .HasForeignKey(x => x.WatchDateCalendar_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.WatchDate).IsRequired();
            this.Property(x => x.WatchDateCalendar_Id).IsRequired();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblwatchdatecalendarvalue");
        }
    }

    public class WatchDateCalendarConfiguration : EntityTypeConfiguration<WatchDateCalendar>
    {
        internal WatchDateCalendarConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.Name).IsRequired().HasColumnName("WatchDateCalendarName");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblwatchdatecalendar");
        }
    }
}
