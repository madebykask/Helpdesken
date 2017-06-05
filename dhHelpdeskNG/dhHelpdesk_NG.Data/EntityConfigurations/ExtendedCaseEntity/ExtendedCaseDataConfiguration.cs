namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.ExtendedCaseEntity;

    internal sealed class ExtendedCaseDataConfiguration : EntityTypeConfiguration<ExtendedCaseDataEntity>
    {
        #region Constructors and Destructors

        internal ExtendedCaseDataConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(e => e.ExtendedCaseGuid).IsRequired();

            this.HasRequired(t => t.ExtendedCaseForm)
                .WithMany(t => t.ExtendedCaseDatas)
                .HasForeignKey(d => d.ExtendedCaseFormId).WillCascadeOnDelete(false);

            ToTable("ExtendedCaseData");
        }

        #endregion
    }
}