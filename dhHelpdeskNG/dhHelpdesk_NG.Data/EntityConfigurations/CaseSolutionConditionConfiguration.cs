namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseSolutionConditionConfiguration : EntityTypeConfiguration<CaseSolutionConditionEntity>
    {
        #region Constructors and Destructors

        internal CaseSolutionConditionConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.CaseSolution_Id).IsRequired();
            Property(e => e.CaseSolutionConditionGUID).IsRequired();
            Property(e => e.CaseField_Name).IsRequired().HasMaxLength(100);
            Property(e => e.Values).IsRequired().HasMaxLength(100);
            Property(e => e.Sequence).IsRequired();
            Property(e => e.Status).IsRequired();
            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsRequired();

            ToTable("tblCaseSolutionCondition");
        }

        #endregion
    }
}