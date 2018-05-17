using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.EntityConfigurations.GDPR
{
    public class GDPRDataPrivacyAccessConfiguration : EntityTypeConfiguration<GDPRDataPrivacyAccess>
    {
        public GDPRDataPrivacyAccessConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.User_Id).IsOptional();
            this.Property(x => x.CreatedDate).IsRequired();

            this.HasOptional(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.User_Id)
                .WillCascadeOnDelete(false);

            this.ToTable("tblGDPRDataPrivacyAccess");
        }
    }
}