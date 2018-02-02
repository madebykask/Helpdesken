using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.EntityConfigurations
{
	public class CaseSolution_CaseSection_ExtendedCaseFormConfiguration : EntityTypeConfiguration<CaseSolution_CaseSection_ExtendedCaseForm>
	{

		internal CaseSolution_CaseSection_ExtendedCaseFormConfiguration()
		{
			this.HasKey(o => o.Id);

			this.Property(o => o.CaseSolutionID).HasColumnName("tblCaseSolutionID").IsRequired();
			this.Property(o => o.CaseSectionID).HasColumnName("tblCaseSectionID").IsRequired();

			this.HasRequired(x => x.CaseSolution)
				.WithMany(x => x.CaseSectionsExtendedCaseForm)
				.HasForeignKey(x => x.CaseSolutionID)
				.WillCascadeOnDelete(false);

			this.HasRequired(x => x.CaseSection)
				.WithMany()
				.HasForeignKey(x => x.CaseSectionID)
				.WillCascadeOnDelete(false);

			this.HasRequired(x => x.ExtendedCaseForm)
				.WithMany()
				.HasForeignKey(x => x.ExtendedCaseFormID)
				.WillCascadeOnDelete(false);



			this.ToTable("tblCaseSolution_tblCaseSection_ExtendedCaseForm");
		}
	}
}
