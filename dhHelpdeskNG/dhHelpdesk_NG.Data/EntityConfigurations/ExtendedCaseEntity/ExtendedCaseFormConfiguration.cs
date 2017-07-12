namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain.ExtendedCaseEntity;

    internal sealed class ExtendedCaseFormConfiguration : EntityTypeConfiguration<ExtendedCaseFormEntity>
    {
        #region Constructors and Destructors

        internal ExtendedCaseFormConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Name).IsOptional().HasMaxLength(100);

            Property(e => e.Version).IsRequired();

            ToTable("ExtendedCaseForms");
        }

        #endregion
    }
}