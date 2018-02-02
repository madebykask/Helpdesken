using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.EntityConfigurations.ExtendedCaseEntity
{
	internal sealed class Case_CaseSection_ExtendedCaseConfiguration : EntityTypeConfiguration<Case_CaseSection_ExtendedCase>
	{
		#region Constructors and Destructors

		internal Case_CaseSection_ExtendedCaseConfiguration()
		{
			HasKey(e => new { e.Case_Id, e.ExtendedCaseData_Id, e.CaseSection_Id });

			HasRequired(t => t.CaseEntity)
				.WithMany(t => t.CaseSectionExtendedCaseDatas)
				.HasForeignKey(d => d.Case_Id)
				.WillCascadeOnDelete(true);


			HasRequired(t => t.ExtendedCaseData)
				.WithMany(t => t.CaseSectionExtendedCaseDatas)
				.HasForeignKey(d => d.ExtendedCaseData_Id)
				.WillCascadeOnDelete(false);


			HasRequired(t => t.CaseSection)
				.WithMany(t => t.Case_CaseSection_ExtendedCases)
				.HasForeignKey(d => d.CaseSection_Id)
				.WillCascadeOnDelete(false);

			ToTable("tblCase_tblCaseSection_ExtendedCaseData");
		}

		#endregion
	}
}
