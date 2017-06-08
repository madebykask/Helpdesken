namespace DH.Helpdesk.Dal.EntityConfigurations.ADFS
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseSolutionConditionPropertyConfiguration : EntityTypeConfiguration<CaseSolutionConditionPropertyEntity>
    {
        #region Constructors and Destructors

        internal CaseSolutionConditionPropertyConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);            
            Property(e => e.CaseSolutionConditionProperty).IsRequired().HasMaxLength(100);            
            Property(e => e.Text).IsOptional().HasMaxLength(400);
            

            ToTable("tblCaseSolutionConditionProperties");
        }

        #endregion
    }
}