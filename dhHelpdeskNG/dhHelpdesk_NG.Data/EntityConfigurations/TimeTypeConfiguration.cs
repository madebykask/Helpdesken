namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class TimeTypeConfiguration : EntityTypeConfiguration<TimeType>
    {
        internal TimeTypeConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Code).IsOptional().HasMaxLength(5).HasColumnName("TimeTypeCode");
            this.Property(x => x.Invoice).IsRequired();
            this.Property(x => x.InvoiceTimeFromRegisteredTime).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(50).HasColumnName("TimeType");
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tbltimetype");
        }
    }
}
