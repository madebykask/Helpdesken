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
	internal sealed class ExtendedCaseReportFieldConfiguration : EntityTypeConfiguration<ExtendedCaseReportField>
	{
		internal ExtendedCaseReportFieldConfiguration()
		{
			HasKey(e => e.Id);
			Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.HasRequired(x => x.ExtendedCaseReport)
				.WithMany()
				.HasForeignKey(x => x.ExtendedCaseReport_Id);

			this.Property(x => x.FieldId).IsRequired();
			this.Property(x => x.Name).IsRequired();
			this.Property(x => x.SortOrder).IsRequired();

			ToTable("ExtendedCaseReportField");
		}
	}
}
