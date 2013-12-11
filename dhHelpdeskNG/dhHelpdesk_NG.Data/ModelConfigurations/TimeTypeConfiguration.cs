using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class TimeTypeConfiguration : EntityTypeConfiguration<TimeType>
    {
        internal TimeTypeConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Code).IsOptional().HasMaxLength(5).HasColumnName("TimeTypeCode");
            Property(x => x.Invoice).IsRequired();
            Property(x => x.InvoiceTimeFromRegisteredTime).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("TimeType");
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("tbltimetype");
        }
    }
}
