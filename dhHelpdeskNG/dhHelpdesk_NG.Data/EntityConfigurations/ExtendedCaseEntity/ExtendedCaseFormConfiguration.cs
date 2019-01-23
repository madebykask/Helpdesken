using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Dal.EntityConfigurations.ExtendedCaseEntity
{
    internal sealed class ExtendedCaseFormConfiguration : EntityTypeConfiguration<ExtendedCaseFormEntity>
    {
        #region Constructors and Destructors

        internal ExtendedCaseFormConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Name).IsOptional().HasMaxLength(100);
            Property(e => e.MetaData).HasMaxLength(null);
            Property(e => e.CreatedBy).IsRequired().HasMaxLength(50);
            Property(e => e.UpdatedBy).IsOptional().HasMaxLength(50);
            Property(e => e.CreatedOn).IsRequired();
            Property(e => e.UpdatedBy).IsOptional();

            Property(e => e.Version).IsRequired();

            ToTable("ExtendedCaseForms");
        }

        #endregion
    }
}