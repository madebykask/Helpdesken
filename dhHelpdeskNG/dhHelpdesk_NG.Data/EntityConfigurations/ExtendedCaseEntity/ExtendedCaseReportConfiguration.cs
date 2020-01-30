using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.EntityConfigurations.ExtendedCaseEntity
{
	internal sealed class ExtendedCaseReportConfiguration : EntityTypeConfiguration<ExtendedCaseReport>
	{
		internal ExtendedCaseReportConfiguration()
		{
			HasKey(e => e.Id);
			Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.HasMany(u => u.ExtendedCaseReportFields)
				.WithRequired(x => x.ExtendedCaseReport)
				.HasForeignKey(x => x.ExtendedCaseReport_Id);

			this.HasRequired(x => x.Customer)
				.WithMany()
				.HasForeignKey(x => x.Customer_Id);

			this.HasRequired(x => x.ExtendedCaseForm)
				.WithMany()
				.HasForeignKey(x => x.ExtendedCaseForm_Id);

			ToTable("ExtendedCaseReport");
		}
	}
}
