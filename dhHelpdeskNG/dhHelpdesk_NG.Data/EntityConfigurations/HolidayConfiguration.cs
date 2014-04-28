namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class HolidayConfiguration : EntityTypeConfiguration<Holiday>
    {
        internal HolidayConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.HolidayHeader)
                .WithMany(o => o.Holidays)
                .HasForeignKey(o => o.HolidayHeader_Id)
                .WillCascadeOnDelete(false);

            this.Property(x => x.HolidayDate).IsRequired().HasColumnName("Holiday");
            this.Property(x => x.HolidayHeader_Id).IsRequired();
            this.Property(x => x.TimeFrom).IsRequired();
            this.Property(x => x.TimeUntil).IsOptional();
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblholiday");
        }
    }

    public class HolidayHeaderConfiguration : EntityTypeConfiguration<HolidayHeader>
    {
        internal HolidayHeaderConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasColumnName("HolidayHeaderName");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblholidayheader");
        }
    }
}
