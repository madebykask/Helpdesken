namespace DH.Helpdesk.Dal.EntityConfigurations.ExtendedCaseEntity
{        
    using System.Data.Entity.ModelConfiguration;
    using Domain.ExtendedCaseEntity;

    internal sealed class Case_ExtendedCaseDataConfiguration : EntityTypeConfiguration<Case_ExtendedCaseEntity>
    {
        #region Constructors and Destructors

        internal Case_ExtendedCaseDataConfiguration()
        {
            HasKey(e => new { e.Case_Id, e.ExtendedCaseData_Id });
                        
            HasRequired(t => t.CaseEntity)
                .WithMany(t => t.CaseExtendedCaseDatas)
                .HasForeignKey(d => d.Case_Id)
                .WillCascadeOnDelete(true);


            HasRequired(t => t.ExtendedCaseData)
                .WithMany(t => t.CaseExtendedCaseDatas)
                .HasForeignKey(d => d.ExtendedCaseData_Id)
                .WillCascadeOnDelete(false);


            HasOptional(t => t.ExtendedCaseForm)
                .WithMany(t => t.Case_ExtendedCases)
                .HasForeignKey(d => d.ExtendedCaseForm_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblCase_ExtendedCaseData");
        }

        #endregion
    }
}