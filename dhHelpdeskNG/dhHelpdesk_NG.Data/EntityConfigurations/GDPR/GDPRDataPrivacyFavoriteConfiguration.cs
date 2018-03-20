using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.EntityConfigurations.GDPR
{
    public class GDPRDataPrivacyFavoriteConfiguration : EntityTypeConfiguration<GDPRDataPrivacyFavorite>
    {
        public GDPRDataPrivacyFavoriteConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Name).IsRequired().HasMaxLength(256);

            this.Property(x => x.CustomerId).IsRequired();
            this.Property(x => x.RetentionPeriod).IsRequired();
            this.Property(x => x.CalculateRegistrationDate).IsRequired();
            this.Property(x => x.RegisterDateFrom).IsRequired();
            this.Property(x => x.RegisterDateTo).IsRequired();

            this.Property(x => x.ClosedOnly).IsRequired();
            this.Property(x => x.FieldsNames).IsRequired().HasMaxLength(512);
            this.Property(x => x.ReplaceDataWith).IsRequired().HasMaxLength(256);

            this.Property(x => x.ReplaceDatesWith).IsOptional();
            this.Property(x => x.RemoveCaseAttachments).IsRequired();
            this.Property(x => x.RemoveCaseAttachments).IsRequired();

            this.ToTable("tblGDPRDataPrivacyFavorite");
        }
    }
}