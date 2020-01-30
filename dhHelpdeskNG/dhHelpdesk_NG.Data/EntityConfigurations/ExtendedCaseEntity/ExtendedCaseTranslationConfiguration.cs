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
	internal sealed class ExtendedCaseTranslationConfiguration : EntityTypeConfiguration<ExtendedCaseTranslationEntity>
	{
		#region Constructors and Destructors

		internal ExtendedCaseTranslationConfiguration()
		{
			HasKey(e => e.Id);
			Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			Property(e => e.LanguageId).IsRequired();
			Property(e => e.Property).IsRequired();
			Property(e => e.Text).IsRequired();


			ToTable("ExtendedCaseTranslations");
		}

		#endregion
	}
}
