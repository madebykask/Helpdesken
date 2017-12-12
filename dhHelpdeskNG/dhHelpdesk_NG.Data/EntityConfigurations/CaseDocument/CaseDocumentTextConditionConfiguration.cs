namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseDocumentTextConditionConfiguration : EntityTypeConfiguration<CaseDocumentTextConditionEntity>
    {
        #region Constructors and Destructors

        internal CaseDocumentTextConditionConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.CaseDocumentText_Id).IsRequired();
            Property(e => e.CaseDocumentTextConditionGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(e => e.Property_Name).IsRequired().HasMaxLength(100);
            Property(e => e.Values).IsRequired();
            Property(e => e.Description).IsOptional().HasMaxLength(200);
            Property(e => e.Operator).IsRequired().HasMaxLength(50);
            Property(e => e.CreatedByUser_Id).IsOptional();
            Property(e => e.ChangedByUser_Id).IsOptional();
            Property(e => e.Status).IsRequired();
            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsRequired();
            Property(e => e.SortOrder).IsRequired();
            Property(e => e.Name).IsOptional().HasMaxLength(200);

            ToTable("tblCaseDocumentTextCondition");
        }

        #endregion
    }
}