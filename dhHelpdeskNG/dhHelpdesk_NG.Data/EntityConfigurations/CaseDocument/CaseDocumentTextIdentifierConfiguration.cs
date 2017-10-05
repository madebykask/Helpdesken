namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseDocumentTextIdentifierConfiguration : EntityTypeConfiguration<CaseDocumentTextIdentifierEntity>
    {
        #region Constructors and Destructors

        internal CaseDocumentTextIdentifierConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.ExtendedCaseFormId).IsRequired();
            Property(e => e.Identifier).IsRequired();
            Property(e => e.PropertyName).IsRequired();
            Property(e => e.DisplayName).IsOptional();

            Property(e => e.DisplayFormat).IsOptional();
            Property(e => e.DataType).IsOptional();
            Property(e => e.DataFormat).IsOptional();

            ToTable("tblCaseDocumentTextIdentifier");
        }

        #endregion
    }
}