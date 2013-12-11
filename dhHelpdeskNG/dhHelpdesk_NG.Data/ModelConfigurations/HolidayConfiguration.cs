using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class HolidayConfiguration : EntityTypeConfiguration<Holiday>
    {
        internal HolidayConfiguration()
        {
            HasKey(x => x.Id);

            HasRequired(x => x.HolidayHeader)
                .WithMany(o => o.Holidays)
                .HasForeignKey(o => o.HolidayHeader_Id)
                .WillCascadeOnDelete(false);

            Property(x => x.HolidayDate).IsRequired().HasColumnName("Holiday");
            Property(x => x.HolidayHeader_Id).IsRequired();
            Property(x => x.TimeFrom).IsRequired();
            Property(x => x.TimeUntil).IsOptional();
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblholiday");
        }
    }

    public class HolidayHeaderConfiguration : EntityTypeConfiguration<HolidayHeader>
    {
        internal HolidayHeaderConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Name).IsRequired().HasColumnName("HolidayHeaderName");
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tblholidayheader");
        }
    }
}
