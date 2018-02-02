namespace DH.Helpdesk.Dal.EntityConfigurations.ExtendedCaseEntity
{        
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal sealed class CaseSolution_SplitToCaseSolutionConfiguration : EntityTypeConfiguration<CaseSolution_SplitToCaseSolutionEntity>
    {
        #region Constructors and Destructors

        internal CaseSolution_SplitToCaseSolutionConfiguration()
        {
            HasKey(e => new { e.CaseSolution_Id, e.SplitToCaseSolution_Id });

            HasRequired(t => t.CaseSolution)
                .WithMany(t => t.SplitToCaseSolutionDescendants)
                .HasForeignKey(d => d.CaseSolution_Id)
                .WillCascadeOnDelete(true);
            

            HasRequired(t => t.SplitToCaseSolutionDescendant)
                .WithMany(t => t.SplitToCaseSolutionAnsestors)
                .HasForeignKey(d => d.SplitToCaseSolution_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblCaseSolution_SplitToCaseSolution");
        }

        #endregion
    }
}