using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using DH.Helpdesk.Domain;
    public class FormUrlConfiguration : EntityTypeConfiguration<FormUrlEntity>
    {
        internal FormUrlConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.ExternalSite).IsRequired();
            this.Property(x => x.Scheme).IsRequired().HasMaxLength(10);
            this.Property(x => x.Host).IsRequired().HasMaxLength(1000);
            this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.ToTable("tblFormUrl");
        }
    }
}
