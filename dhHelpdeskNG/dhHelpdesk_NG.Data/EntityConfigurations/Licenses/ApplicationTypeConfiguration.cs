using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations.Licenses
{
    internal sealed class ApplicationTypeConfiguration : EntityTypeConfiguration<ApplicationTypeEntity>
    {
        internal ApplicationTypeConfiguration()
        {
            this.HasKey(a => a.Id);
            this.Property(a => a.Type).HasColumnName("ApplicationType").IsRequired().HasMaxLength(50);
            this.Property(a => a.UniqueId).IsRequired().HasColumnName("ApplicationTypeGUID");

            this.ToTable("tblApplicationType");
        }
    }
}