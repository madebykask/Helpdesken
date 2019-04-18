using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
	public class FeatureToggleConfiguration : EntityTypeConfiguration<FeatureToggle>
	{
		internal FeatureToggleConfiguration()
		{
			this.HasKey(x => x.StrongName);

			this.Property(x => x.Active).IsRequired();
			this.Property(x => x.Description).IsRequired();
			this.Property(x => x.ChangeDate).IsRequired();

			this.ToTable("tblFeatureToggle");
		}
	}
}
