using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Dal.EntityConfigurations.ExtendedCaseEntity
{
    internal sealed class ExtendedCaseValueConfiguration : EntityTypeConfiguration<ExtendedCaseValueEntity>
    {
        #region Constructors and Destructors

        internal ExtendedCaseValueConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.ExtendedCaseDataId).IsRequired();
            Property(e => e.FieldId).IsRequired();
            Property(e => e.Value).IsOptional();
            Property(e => e.SecondaryValue).IsOptional();

            //LAZY
            this.HasRequired(c => c.ExtendedCaseData)
            .WithMany(c => c.ExtendedCaseValues)
            .HasForeignKey(c => c.ExtendedCaseDataId)
            .WillCascadeOnDelete(false);

			ToTable("ExtendedCaseValues");
        }

        #endregion
    }
}