using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Dal.EntityConfigurations.ExtendedCaseEntity
{
    internal sealed class ExtendedCaseDataConfiguration : EntityTypeConfiguration<ExtendedCaseDataEntity>
    {
        #region Constructors and Destructors

        internal ExtendedCaseDataConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.ExtendedCaseGuid).IsRequired();
            Property(e => e.ExtendedCaseFormId).IsRequired();
            Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            Property(e => e.CreatedOn).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(e => e.UpdatedBy).IsOptional().HasMaxLength(50);
            Property(e => e.UpdatedOn).IsOptional();


            HasRequired(t => t.ExtendedCaseForm)
				.WithMany(t => t.ExtendedCaseDatas)
				.HasForeignKey(d => d.ExtendedCaseFormId).WillCascadeOnDelete(false);



            ToTable("ExtendedCaseData");
        }

        #endregion
    }
}