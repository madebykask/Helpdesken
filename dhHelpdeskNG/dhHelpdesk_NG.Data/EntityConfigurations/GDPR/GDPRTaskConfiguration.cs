using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.EntityConfigurations.GDPR
{
    public class GDPRTaskConfiguration : EntityTypeConfiguration<GDPRTask>
    {
        public GDPRTaskConfiguration()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.CustomerId).IsOptional();
            this.Property(x => x.UserId).IsOptional();
            this.Property(x => x.FavoriteId).IsOptional();
            this.Property(x => x.Status).IsOptional();
            this.Property(x => x.AddedDate).IsOptional();
            this.Property(x => x.Progress).IsOptional();

            this.Property(x => x.StartedAt).IsOptional();
            this.Property(x => x.EndedAt).IsOptional();
            this.Property(x => x.Success).IsRequired();
            this.Property(x => x.Error).HasMaxLength(512).IsOptional();

            this.ToTable("tblGDPRTask");
        }
    }
}