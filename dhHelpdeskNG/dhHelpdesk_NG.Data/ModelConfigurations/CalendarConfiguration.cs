using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class CalendarConfiguration : EntityTypeConfiguration<dhHelpdesk_NG.Domain.Calendar>
    {
        internal CalendarConfiguration()
        {
            HasKey(x => x.Id);

            HasMany(u => u.WGs)
                .WithMany(a => a.Calendars)
                .Map(m =>
                    {
                        m.MapLeftKey("Calendar_Id");
                        m.MapRightKey("WorkingGroup_Id");
                        m.ToTable("tblCalendar_tblWorkingGroup");
                    }
                );

            HasRequired(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            Property(x => x.CalendarDate).IsRequired();
            Property(x => x.Caption).IsRequired().HasMaxLength(50).HasColumnName("CalendarCaption");
            Property(x => x.ChangedByUser_Id).IsRequired();
            Property(x => x.Customer_Id).IsRequired();
            Property(x => x.PublicInformation).IsRequired();
            Property(x => x.ShowOnStartPage).IsRequired();
            Property(x => x.ShowUntilDate).IsRequired();
            Property(x => x.Text).IsRequired().HasMaxLength(2000).HasColumnName("CalendarText");
            Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblcalendar");
        }
    }
}