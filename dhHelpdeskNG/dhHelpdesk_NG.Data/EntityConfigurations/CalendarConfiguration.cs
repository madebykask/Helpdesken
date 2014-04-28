namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class CalendarConfiguration : EntityTypeConfiguration<Calendar>
    {
        internal CalendarConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasMany(u => u.WGs)
                .WithMany(a => a.Calendars)
                .Map(m =>
                    {
                        m.MapLeftKey("Calendar_Id");
                        m.MapRightKey("WorkingGroup_Id");
                        m.ToTable("tblCalendar_tblWorkingGroup");
                    }
                );

            this.HasRequired(x => x.ChangedByUser)
                .WithMany()
                .HasForeignKey(x => x.ChangedByUser_Id)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Customer)
               .WithMany()
               .HasForeignKey(x => x.Customer_Id)
               .WillCascadeOnDelete(false);

            this.Property(x => x.CalendarDate).IsRequired();
            this.Property(x => x.Caption).IsRequired().HasMaxLength(50).HasColumnName("CalendarCaption");
            this.Property(x => x.ChangedByUser_Id).IsRequired();
            this.Property(x => x.Customer_Id).IsRequired();
            this.Property(x => x.PublicInformation).IsRequired();
            this.Property(x => x.ShowOnStartPage).IsRequired();
            this.Property(x => x.ShowUntilDate).IsRequired();
            this.Property(x => x.Text).IsRequired().HasMaxLength(2000).HasColumnName("CalendarText");
            this.Property(x => x.ChangedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblcalendar");
        }
    }
}