
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.ModelConfigurations
{
    public class OperatingSystemConfiguration : EntityTypeConfiguration<OperatingSystem>
    {
        internal OperatingSystemConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasColumnName("OperatingSystem");
            ToTable("tbloperatingsystem");
        }
    }
}
