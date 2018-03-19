using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.EntityConfigurations.GDPR
{
    public class GDPRDataPrivacyFavortieConfiguration : EntityTypeConfiguration<GDPRDataPrivacyFavorite>
    {
        public GDPRDataPrivacyFavortieConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.Name).IsRequired().HasMaxLength(256);

            this.ToTable("tblGDPRDataPrivacyFavorite");
        }
    }
}